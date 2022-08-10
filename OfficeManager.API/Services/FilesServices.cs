using OfficeManager.Application.Common.Interfaces;
using OfficeManager.Application.Employees.Commands.AddBulkEmployees;
using OfficeOpenXml;

namespace OfficeManager.API.Services
{
    public class FilesServices : IFilesServices
    {
        public async Task<List<BIEmployeeDto>> ReadEmployeeExcel(string path)
        {
            List<BIEmployeeDto> employees = new List<BIEmployeeDto>();
            using (ExcelPackage package = new ExcelPackage(new FileInfo(path)))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets["Jul 2022"];
                int totalRows = worksheet.Dimension.Rows;
                
                for(int index=4; index < totalRows && worksheet.Cells[index,1].Value != null; index++)
                {

                    BIEmployeeDto employee = new BIEmployeeDto()
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
