using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OfficeManager.Application.Common.Models;
using OfficeManager.Application.Departments.Commands.CreateDepartmentCommand;
using OfficeManager.Application.Departments.Commands.DeleteDepartmentCommand;
using OfficeManager.Application.Departments.Queries.GetAllDepartmentsQuery;

namespace OfficeManager.API.Controllers
{
    [Authorize]
    public class DepartmentController : ApiControllerBase
    {
        [HttpGet]
        [Route("")]
        public async Task<List<DepartmentDto>> GetAll(string? search)
        {
            return await Mediator.Send(new GetAllDepartmentsQuery(search));
        }

        [HttpPost]
        [Route("Add")]
        public async Task<ActionResult<Result>> CreateDepartment(CreateDepartmentCommand command)
        {
            return await Mediator.Send(command);
        }

        [HttpDelete]
        [Route("{id}/Delete")]
        public async Task<ActionResult<Result>> DeleteDepartment(Guid id)
        {
            return await Mediator.Send(new DeleteDepartmentCommand(id));
        }
    }
}
