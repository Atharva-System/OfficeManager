using OfficeManager.Application.Common.Interfaces;
using OfficeManager.Application.Employees.Commands.AddBulkEmployees;
using OfficeOpenXml;

namespace OfficeManager.API.Services
{
    public class FilesServices : IFilesServices
    {
        public async Task<List<BulkImportEmployeeDto>> ReadEmployeeExcel(string path)
        {
            List<BulkImportEmployeeDto> employees = new List<BulkImportEmployeeDto>();
            using (ExcelPackage package = new ExcelPackage(new FileInfo(path)))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets["Jul 2022"];
                int totalRows = worksheet.Dimension.Rows;
                
                for(int index=4; index < totalRows && worksheet.Cells[index,1].Value != null; index++)
                {

                    BulkImportEmployeeDto employee = new BulkImportEmployeeDto()
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
            return employees;
        }
    }
}
