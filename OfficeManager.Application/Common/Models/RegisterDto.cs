using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OfficeManager.Application.Common.Models
{
    public class RegisterDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string Contact { get; set; }
        public string PersonalEmail { get; set; }
        public DateTime DateOfJoining { get; set; } = DateTime.Now;
    }
}
