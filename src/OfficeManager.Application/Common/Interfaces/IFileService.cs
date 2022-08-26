using OfficeManager.Application.Common.Models;

namespace OfficeManager.Application.Common.Interfaces
{
    public interface IFileService
    {
        Task<Result> ReadEmployees(File file);
    }
}
