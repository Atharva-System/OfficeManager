using OfficeManager.Application.Common.Interfaces;
using OfficeManager.Application.Dtos;

namespace OfficeManager.Infrastructure.Services
{
    public class CurrentUserService : ICurrentUserServices
    {
        public LoggedInUserDTO loggedInUser { get; set; }
    }
}
