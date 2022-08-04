using OfficeManager.Application.Common.Models;

namespace OfficeManager.Application.Common.Interfaces
{
    public interface IFilesServices
    {
        Task<List<EmployeeDto>> ReadEmployeeExcel(string path);
    }
}
