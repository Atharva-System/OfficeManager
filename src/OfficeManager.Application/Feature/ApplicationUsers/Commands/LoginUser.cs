using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OfficeManager.Application.Common.Interfaces;
using OfficeManager.Application.Common.Models;
using OfficeManager.Application.Dtos;
using OfficeManager.Domain.Entities;

namespace OfficeManager.Application.Feature.ApplicationUsers.Commands
{
    public record LoginUser : IRequest<Response<TokenDTO>>
    {
        public int EmployeeNo { get; init; }
        public string Password { get; init; } = string.Empty;
    }

    public class LoginUserCommandHandler : IRequestHandler<LoginUser, Response<TokenDTO>>
    {
        private readonly IApplicationDbContext Context;
        private readonly IMapper Mapper;
        private readonly ICurrentUserServices CurrentUserService;
        private readonly ITokenService TokenService;
        public LoginUserCommandHandler(IApplicationDbContext context, IMapper mapper, ICurrentUserServices currentUserService, ITokenService tokenService)
        {
            Context = context;
            Mapper = mapper;
            CurrentUserService = currentUserService;
            TokenService = tokenService;
        }

        public async Task<Response<TokenDTO>> Handle(LoginUser request, CancellationToken cancellationToken)
        {
            Response<TokenDTO> response = new Response<TokenDTO>();

            try
            {
                var user = await Context.Users.Include(x => x.Employee)
                .FirstOrDefaultAsync(a => a.Employee.EmployeeNo == request.EmployeeNo);

                if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
                {
                    response.IsSuccess = false;
                    response.Message = Messages.CheckCredentials;
                    response.StatusCode = StausCodes.BadRequest;
                    return response;
                }

                var userRoles = await Context.UserRoleMapping.Include(x => x.Roles).Where(d => d.UserId == user.Id)
                                    .ProjectTo<UserRoleDTO>(Mapper.ConfigurationProvider).ToListAsync();

                LoggedInUserDTO loggedInUser = Mapper.Map<UserMaster, LoggedInUserDTO>(user);
                loggedInUser.Roles = userRoles;
                var tokendto = TokenService.CreateToken(loggedInUser);

                response.Data = tokendto;

                CurrentUserService.loggedInUser = loggedInUser;

                RefreshToken refreshToken = new RefreshToken
                {
                    UserId = loggedInUser.UserId,
                    Code = tokendto.RefreshToken,
                    Expiration = tokendto.RefreshTokenExpiration
                };

                Context.RefreshToken.Add(refreshToken);
                Context.BeginTransaction();
                await Context.SaveChangesAsync(cancellationToken);
                Context.CommitTransaction();

                response.IsSuccess = true;
                response.Message = Messages.Success;
                response.StatusCode = StausCodes.Accepted;

                return response;
            }
            catch (Exception ex)
            {
                response.Message = Messages.IssueWithData;
                response.Errors.Add(ex.Message);
                response.IsSuccess = false;
                response.StatusCode = StausCodes.InternalServerError;
                return response;
            }
            
        }
    }
}
