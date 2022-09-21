using OfficeManager.Application.Dtos;
using OfficeManager.Domain.Entities;

namespace OfficeManager.Application.Common.Interfaces
{
    public interface ITokenService
    {
        TokenDTO CreateToken(LoggedInUserDTO user);

        bool ValidateToken(string token);
    }
}
