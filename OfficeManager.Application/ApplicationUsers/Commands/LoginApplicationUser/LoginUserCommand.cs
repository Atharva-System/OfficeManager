using MediatR;
using Microsoft.EntityFrameworkCore;
using OfficeManager.Application.Common.Interfaces;
using OfficeManager.Domain.Entities;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using OfficeManager.Application.ApplicationRoles.Queries;
using OfficeManager.Application.UserRoles.Queries;

namespace OfficeManager.Application.ApplicationUsers.Commands.LoginApplicationUser
{
    public record LoginUserCommand : IRequest<LoggedInUserDto>
    {
        public int EmployeeNo { get; init; }
        public string Password { get; init; }
    }

    public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, LoggedInUserDto>
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

        public async Task<LoggedInUserDto> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            var user = _context.Users.Include("Employee")
                .FirstOrDefault(a => a.Employee.EmployeeNo == request.EmployeeNo);
            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
                return null;

            var userRoles = await _context.UserRoleMapping.Include("Roles").Where(d => d.UserId == user.Id)
                                .ProjectTo<UserRoleDto>(_mapper.ConfigurationProvider).ToListAsync();
            LoggedInUserDto loggedInUser = _mapper.Map<UserMaster, LoggedInUserDto>(user);
            loggedInUser.Roles = userRoles;

            _currentUserService.loggedInUser = loggedInUser;
            return loggedInUser;

        }
    }
}
