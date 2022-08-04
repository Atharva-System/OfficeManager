using Microsoft.EntityFrameworkCore;
using OfficeManager.Application.ApplicationUsers.Commands.LoginApplicationUser;
using OfficeManager.Application.Employees.Queries.GetAllEmployees;
using OfficeManager.Domain.Entities;

namespace OfficeManager.Application.Common.Interfaces
{
    public interface ICurrentUserServices
    {
        LoggedInUserDto loggedInUser { get; set; }
    }
}
