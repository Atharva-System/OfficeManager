using MediatR;
using Microsoft.EntityFrameworkCore;
using OfficeManager.Application.Common.Interfaces;
using OfficeManager.Domain.Entities;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using OfficeManager.Application.ApplicationRoles.Queries;
using OfficeManager.Application.UserRoles.Queries;
using OfficeManager.Application.Common.Models;

namespace OfficeManager.Application.ApplicationUsers.Commands.LoginApplicationUser
{
    public record LoginUserCommand : IRequest<Response<LoggedInUserDto>>
    {
        public int EmployeeNo { get; init; }
        public string Password { get; init; }
    }

    public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, Response<LoggedInUserDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly ICurrentUserServices _currentUserService;
        public LoginUserCommandHandler(IApplicationDbContext context, IMapper mapper,ICurrentUserServices currentUserService)
        {
            _context = context;
            _mapper = mapper;
            _currentUserService = currentUserService;
        }

        public async Task<Response<LoggedInUserDto>> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            Response<LoggedInUserDto> response = new Response<LoggedInUserDto>();

            var user = _context.Users.Include("Employee")
                .FirstOrDefault(a => a.Employee.EmployeeNo == request.EmployeeNo);
            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            {
                response._IsSuccess = false;
                response._Message = "Please check login credentials";
                response._StatusCode = "400";
                return response;
            }

            var userRoles = await _context.UserRoleMapping.Include("Roles").Where(d => d.UserId == user.Id)
                                .ProjectTo<UserRoleDto>(_mapper.ConfigurationProvider).ToListAsync();
            LoggedInUserDto loggedInUser = _mapper.Map<UserMaster, LoggedInUserDto>(user);
            loggedInUser.Roles = userRoles;

            response._Data = loggedInUser;

            _currentUserService.loggedInUser = loggedInUser;
            return response;

        }
    }
}
