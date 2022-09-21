using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OfficeManager.Application.Common.Exceptions;
using OfficeManager.Application.Common.Interfaces;
using OfficeManager.Application.Common.Models;
using OfficeManager.Application.Dtos;
using OfficeManager.Application.Wrappers.Abstract;
using OfficeManager.Application.Wrappers.Concrete;
using OfficeManager.Domain.Entities;

namespace OfficeManager.Application.Feature.ApplicationUsers.Commands
{
    public record CreateTokenByRefreshToken : IRequest<IResponse>
    {
        public string RefreshToken { get; init; }
        public string CurrentToken { get; set; }
    }

    public class CreateTokenByRefreshTokenHandler : IRequestHandler<CreateTokenByRefreshToken, IResponse>
    {
        private readonly IApplicationDbContext Context;
        private readonly IMapper Mapper;
        private readonly ICurrentUserServices CurrentUserService;
        private readonly ITokenService TokenService;
        public CreateTokenByRefreshTokenHandler(IApplicationDbContext context, IMapper mapper, ICurrentUserServices currentUserService, ITokenService tokenService)
        {
            Context = context;
            Mapper = mapper;
            CurrentUserService = currentUserService;
            TokenService = tokenService;
        }

        public async Task<IResponse> Handle(CreateTokenByRefreshToken request, CancellationToken cancellationToken)
        {
            TokenDTO response = new TokenDTO();
            var refreshToken = Context.RefreshToken.FirstOrDefault(rt => rt.Code == request.RefreshToken && rt.IsActive == true);

            if(refreshToken == null)
            {
                throw new NotFoundException("Refresh Token is not found.");
            }

            if(TokenService.ValidateToken(request.CurrentToken))
            {
                return new ErrorResponse(StatusCodes.BadRequest, "Current token is not expired yet.");
            }

            var user = Context.Users.FirstOrDefault(u => u.Id == refreshToken.UserId);

            var userRoles = await Context.UserRoleMapping.Include(x => x.Roles).Where(d => d.UserId == user.Id)
                                   .ProjectTo<UserRoleDTO>(Mapper.ConfigurationProvider).ToListAsync();

            LoggedInUserDTO loggedInUser = Mapper.Map<UserMaster, LoggedInUserDTO>(user);
            loggedInUser.Roles = userRoles;
            var tokendto = TokenService.CreateToken(loggedInUser);
            

            CurrentUserService.loggedInUser = loggedInUser;

            RefreshToken newRefreshToken = new RefreshToken
            {
                UserId = loggedInUser.UserId,
                Code = tokendto.RefreshToken,
                Expiration = tokendto.RefreshTokenExpiration
            };

            Context.RefreshToken.Add(newRefreshToken);
            Context.BeginTransaction();
            refreshToken.IsActive = false;
            await Context.SaveChangesAsync(cancellationToken);
            Context.CommitTransaction();

            return new DataResponse<TokenDTO>(tokendto, StatusCodes.Accepted);
        }
    }
}
