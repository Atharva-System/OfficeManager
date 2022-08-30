using OfficeManager.Application.Common.Models;

namespace OfficeManager.Application.Common.Interfaces
{
    public interface IFileService
    {
#pragma warning disable CS8897 // Static types cannot be used as parameters
        Task<Result> ReadEmployees(File file);
#pragma warning restore CS8897 // Static types cannot be used as parameters
    }
}
