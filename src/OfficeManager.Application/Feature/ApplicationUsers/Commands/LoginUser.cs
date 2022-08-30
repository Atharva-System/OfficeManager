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
    public record LoginUser : IRequest<Response<LoggedInUserDTO>>
    {
        public int EmployeeNo { get; init; }
        public string Password { get; init; } = string.Empty;
    }

    public class LoginUserCommandHandler : IRequestHandler<LoginUser, Response<LoggedInUserDTO>>
    {
        private readonly IApplicationDbContext context;
        private readonly IMapper mapper;
        private readonly ICurrentUserServices currentUserService;
        public LoginUserCommandHandler(IApplicationDbContext context, IMapper mapper, ICurrentUserServices currentUserService)
        {
            this.context = context;
            this.mapper = mapper;
            this.currentUserService = currentUserService;
        }

        public async Task<Response<LoggedInUserDTO>> Handle(LoginUser request, CancellationToken cancellationToken)
        {
            Response<LoggedInUserDTO> response = new Response<LoggedInUserDTO>();

            var user = context.Users.Include("Employee")
                .FirstOrDefault(a => a.Employee.EmployeeNo == request.EmployeeNo);
            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            {
                response.IsSuccess = false;
                response.Message = Messages.CheckCredentials;
                response.StatusCode = StausCodes.BadRequest;
                return response;
            }

            var userRoles = await context.UserRoleMapping.Include("Roles").Where(d => d.UserId == user.Id)
                                .ProjectTo<UserRoleDTO>(mapper.ConfigurationProvider).ToListAsync();
            LoggedInUserDTO loggedInUser = mapper.Map<UserMaster, LoggedInUserDTO>(user);
            loggedInUser.Roles = userRoles;

            response.Data = loggedInUser;

            currentUserService.loggedInUser = loggedInUser;
            return response;

        }
    }
}
