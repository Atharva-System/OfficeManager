using Microsoft.AspNetCore.Mvc;
using OfficeManager.Application.Common.Models;
using OfficeManager.Application.Departments.Queries.SearchDepartments;
using OfficeManager.Application.Designations.Queries.SearchDesignationsQuery;

namespace OfficeManager.API.Controllers
{
    public class MastersController : ApiControllerBase
    {
        [HttpGet]
        [Route("Departments")]
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
        [HttpGet]
        [Route("Designations")]
        public async Task<ActionResult<Response<List<DesignationDto>>>> SearchDesignation(string? search)
        {
            try
            {
                var result = await Mediator.Send(new SearchDesignationsQuery(search));
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
