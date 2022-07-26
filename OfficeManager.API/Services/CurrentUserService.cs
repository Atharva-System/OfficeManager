using Microsoft.AspNetCore.Mvc;
using OfficeManager.Application.Common.Interfaces;

namespace OfficeManager.API.Services
{
    public class CurrentUserService : ControllerBase, ICurrentUserService
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public CurrentUserService(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        public string UserId { get; set; }

        public string GetUserId => UserId != null ? UserId : string.Empty;
    }
}
