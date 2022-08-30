using OfficeManager.Application.Dtos;

namespace OfficeManager.Application.Common.Interfaces
{
    public interface IFilesServices
    {
        Task<List<BulkImportEmployeeDTO>> ReadEmployeeExcel(string path);
    }
}
