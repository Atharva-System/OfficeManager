﻿using Microsoft.AspNetCore.Identity;

namespace OfficeManager.Application.ApplicationUsers.Commands.LoginApplicationUser
{
    public class LoggedInUserDto
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
        public List<string> Roles { get; set; }
    }
}
