using OfficeManager.Application.Dtos;

namespace OfficeManager.Application.Common.Interfaces
{
    public interface ICurrentUserServices
    {
        LoggedInUserDTO loggedInUser { get; set; }
    }
}
