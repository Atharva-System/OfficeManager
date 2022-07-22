using MediatR;
using OfficeManager.Application.Common.Exceptions;
using OfficeManager.Application.Common.Interfaces;

namespace OfficeManager.Application.UserMasters.Queries.GetUserProfile
{
    public record GetUserProfileQuery(Guid Id) : IRequest<UserProfileDto>;

    public class GetUserProfileQueryHandler : IRequestHandler<GetUserProfileQuery,UserProfileDto>
    {
        private readonly IApplicationDbContext _context;
        public GetUserProfileQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<UserProfileDto> Handle(GetUserProfileQuery request, CancellationToken cancellationToken)
        {
            var user = _context.UserMaster.FirstOrDefault(u => u.Id == request.Id);

            if(user == null)
            {
                throw new NotFoundException();
            }

            user.Profile = _context.UserProfile.FirstOrDefault(u => u.UserId == user.Id);
            user.Role = _context.UserRole.FirstOrDefault(r => r.Id == user.RoleId);

            var response = new UserProfileDto
            {
                Id = request.Id,
                Email = user.Email,
                Role = user.Role,
                Contact = user.Profile.Contact,
                DateOfJoining = user.Profile.DateOfJoining.Value,
                PersonalEmail = user.Profile.PersonalEmail,
            };

            return response;
        }
    }
}
