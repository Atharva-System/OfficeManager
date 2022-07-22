using OfficeManager.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OfficeManager.Application.UserMasters.Queries.GetUserProfile
{
    public class UserProfileDto
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string Contact { get; set; }
        public UserRole Role { get; set; }
        public string PersonalEmail { get; set; }
        public DateTime DateOfJoining { get; set; }
    }
}
