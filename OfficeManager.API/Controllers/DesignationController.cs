using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OfficeManager.Application.Common.Models;
using OfficeManager.Application.Designations.Commands.CreateDesignationCommand;
using OfficeManager.Application.Designations.Commands.DeleteDesignationCommand;
using OfficeManager.Application.Designations.Queries.GetAllDesignationsQuery;

namespace OfficeManager.API.Controllers
{
    [Authorize]
    public class DesignationController : ApiControllerBase
    {
        [HttpGet]
        [Route("")]
        public async Task<ActionResult<List<DesignationDto>>> GetAll(string? search)
        {
            return await Mediator.Send(new GetAllDesignationQuery(search));
        }

        [HttpPost]
        [Route("Add")]
        public async Task<ActionResult<Result>> CreateDesignation(CreateDesignationCommand command)
        {
            return await Mediator.Send(command);
        }

        [HttpDelete]
        [Route("{id}/Delete")]
        public async Task<ActionResult<Result>> DeleteDesignation(Guid id)
        {
            return await Mediator.Send(new DeleteDesignationCommand(id));
        }
    }
}
