using Dapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using OfficeManager.Application.ApplicationUsers.Commands.LoginApplicationUser;
using OfficeManager.Application.Common.Interfaces;
using OfficeManager.Application.Employees.Queries.GetAllEmployees;
using OfficeManager.Domain.Entities;
using OfficeManager.Infrastructure.Persistence;

namespace OfficeManager.Infrastructure.Services
{
    public class CurrentUserService : ICurrentUserServices
    {
        public LoggedInUserDto loggedInUser { get; set; }
    }
}
