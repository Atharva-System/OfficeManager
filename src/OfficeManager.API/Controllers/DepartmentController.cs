using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OfficeManager.Application.Feature.Departments.Queries;
using OfficeManager.Application.Feature.Employees.Commands;
using OfficeManager.Application.Wrappers.Abstract;
using Swashbuckle.AspNetCore.Annotations;

namespace OfficeManager.API.Controllers
{
    //[Authorize]
    public class DepartmentController : ApiControllerBase
    {
        //return all the deaprtments usefull for dropdown like usage
        [HttpGet]
        [Route("All")]
        //[SwaggerOperation(Summary = "Write your summary here", Description = "Write your Description here")]
        public async Task<IResponse> GetAll()
        
        {
            return await Mediator.Send(new GetAllDepartments());
        }

        //return paginated result useful for search and listing features
        [HttpGet]
        [Route("")]
        public async Task<IResponse> Search([FromQuery] SearchDepartments query)
        {
            return await Mediator.Send(query);
        }

        [HttpPost]
        [Route("Add")]
        public async Task<IResponse> AddDepartment(AddDepartment command)
        {
            return await Mediator.Send(command);
        }

        [HttpPut]
        [Route("Update")]
        public async Task<IResponse> UpdateDepartment(UpdateDepartment command)
        {
            return await Mediator.Send(command);
        }

        [HttpGet]
        [Route("Filter")]
        public async Task<IResponse> FilterDepartmennt([FromQuery] FilterDepartments query)
        {
            return await Mediator.Send(query);
        }
    }
}