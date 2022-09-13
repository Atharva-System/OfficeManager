using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OfficeManager.Application.Common.Models;
using OfficeManager.Application.Dtos;
using OfficeManager.Application.Feature.Departments.Queries;
using OfficeManager.Application.Wrappers.Abstract;

namespace OfficeManager.API.Controllers
{
    [Authorize]
    public class DepartmentController : ApiControllerBase
    {
        //return all the deaprtments usefull for dropdown like usage
        [HttpGet]
        [Route("All")]
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
    }
}