using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OfficeManager.Application.Common.Interfaces;
using OfficeManager.Application.Common.Models;
using OfficeManager.Application.Dtos;
using OfficeManager.Domain.Entities;

namespace OfficeManager.Application.ApplicationUsers.Commands.LoginApplicationUser
{
    public record LoginUserCommand : IRequest<Response<LoggedInUserDto>>
    {
        public int EmployeeNo { get; init; }
        public string Password { get; init; } = string.Empty;
    }

    public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, Response<LoggedInUserDto>>
    {
        private readonly IApplicationDbContext context;
        private readonly IMapper mapper;
        private readonly ICurrentUserServices currentUserService;
        public LoginUserCommandHandler(IApplicationDbContext context, IMapper mapper,ICurrentUserServices currentUserService)
        {
            this.context = context;
            this.mapper = mapper;
            this.currentUserService = currentUserService;
        }

        public async Task<Response<LoggedInUserDto>> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            Response<LoggedInUserDto> response = new Response<LoggedInUserDto>();

            var user = context.Users.Include("Employee")
                .FirstOrDefault(a => a.Employee.EmployeeNo == request.EmployeeNo);
            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            {
                response.IsSuccess = false;
                response.Message = "Please check login credentials";
                response.StatusCode = "400";
                return response;
            }

            var userRoles = await context.UserRoleMapping.Include("Roles").Where(d => d.UserId == user.Id)
                                .ProjectTo<UserRoleDTO>(mapper.ConfigurationProvider).ToListAsync();
            LoggedInUserDto loggedInUser = mapper.Map<UserMaster, LoggedInUserDto>(user);
            loggedInUser.Roles = userRoles;

            response.Data = loggedInUser;

            currentUserService.loggedInUser = loggedInUser;
            return response;

        }
    }
}
