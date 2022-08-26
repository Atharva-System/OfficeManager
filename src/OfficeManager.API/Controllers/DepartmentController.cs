using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OfficeManager.Application.Common.Models;
using OfficeManager.Application.Departments.Queries.SearchDepartments;

namespace OfficeManager.API.Controllers
{
    [Authorize]
    public class DepartmentController : ApiControllerBase
    {
        [HttpGet]
        [Route("GetAllDepartment")]
        public async Task<ActionResult<Response<List<DepartmentDto>>>> SearchDepartment(string? search)
        {
            try
            {
                var result = await Mediator.Send(new SearchDepartmentsQuery(search));
                if (result.Data == null)
                    NotFound("No records found");
                return result;
            }
            catch (Exception ex)
            {
                return BadRequest("Search has some issue please check internet connection.");
            }
        }
    }
}