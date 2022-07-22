using Microsoft.AspNetCore.Mvc;
using OfficeManager.Application.Common.Interfaces;
using System.Security.Claims;

namespace OfficeManager.API.Services
{
    public class CurrentUserService : ControllerBase, ICurrentUserService
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public CurrentUserService(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        public Guid UserId { get; set; }

        public Guid GetUserId => UserId != null ? UserId : Guid.Empty;
    }
}
