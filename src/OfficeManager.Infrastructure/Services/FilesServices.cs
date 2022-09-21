using OfficeManager.Application.Dtos;
using OfficeManager.Application.Common.Interfaces;

namespace OfficeManager.Infrastructure.Services
{
    public class FilesServices : IFilesServices
    {
        public Task<List<BulkImportEmployeeDTO>> ReadEmployeeExcel(string path)
        {
            List<BulkImportEmployeeDTO> employees = new List<BulkImportEmployeeDTO>();
            using (OfficeOpenXml.ExcelPackage package = new OfficeOpenXml.ExcelPackage(new FileInfo(path)))
            {
                OfficeOpenXml.ExcelWorksheet worksheet = package.Workbook.Worksheets[1];
                int totalRows = worksheet.Dimension.Rows;
                
                for(int index=4; index < totalRows && worksheet.Cells[index,1].Value != null; index++)
                {

                    BulkImportEmployeeDTO employee = new BulkImportEmployeeDTO()
                    {
                        EmployeeNo = Convert.ToInt32(worksheet.Cells[index, 1].Value),
                        EmployeeName = worksheet.Cells[index, 2].Value.ToString(),
                        DateOfJoining = Convert.ToDateTime((worksheet.Cells[index, 3].Value)),
                        Designation = worksheet.Cells[index, 4].Value.ToString(),
                        Department = worksheet.Cells[index, 5].Value.ToString(),
                        DateOfBirth = Convert.ToDateTime((worksheet.Cells[index, 6].Value))
                    };
                    employees.Add(employee);
                }
            }
            return Task.FromResult(employees);
        }
    }
}
