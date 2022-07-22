using OfficeManager.Domain.Entities;

namespace OfficeManager.Application.UserMasters.Commands.AuthenticateUser
{
    public class LoggedInUserDto
    {
        public Guid UserId { get; set; }
        public string Email { get; set; }
        public string Contact { get; set; }
        public string Token { get; set; }
        public UserRole Role { get; set; }
    }
}
