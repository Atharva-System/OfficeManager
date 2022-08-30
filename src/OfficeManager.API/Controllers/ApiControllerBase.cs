using MediatR;

using Microsoft.AspNetCore.Mvc;

namespace OfficeManager.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ApiControllerBase : ControllerBase
    {
        private ISender mediator = null!;

        protected ISender Mediator => mediator ?? HttpContext.RequestServices.GetRequiredService<ISender>();
    }
}
