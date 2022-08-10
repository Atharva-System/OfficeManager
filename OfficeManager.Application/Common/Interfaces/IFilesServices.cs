using OfficeManager.Application.Employees.Commands.AddBulkEmployees;

namespace OfficeManager.Application.Common.Interfaces
{
    public interface IFilesServices
    {
        Task<List<BIEmployeeDto>> ReadEmployeeExcel(string path);
    }
}
