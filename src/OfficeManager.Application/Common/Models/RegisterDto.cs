namespace OfficeManager.Application.Common.Models
{
    public class RegisterDto
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Contact { get; set; } = string.Empty;
        public string PersonalEmail { get; set; } = string.Empty;
        public DateTime DateOfJoining { get; set; } = DateTime.Now;
    }
}
