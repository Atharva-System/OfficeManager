using OfficeManager.Application.Dtos;

namespace OfficeManager.Application.Common.Interfaces
{
    public interface ITokenService
    {
        TokenDTO CreateToken(LoggedInUserDTO user);

        bool ValidateToken(string token);
    }
}
