using Microsoft.AspNetCore.Identity;
using OfficeManager.Application.Common.Models;

namespace OfficeManager.Infrastructure.Identity
{
    public static class IdentityResultExtensions
    {
        public static Result ToApplicationResult(this IdentityResult result)
        {
            return result.Succeeded
                ? Result.Success("Success")
                : Result.Failure(result.Errors.Select(e => e.Description),"Authentication failed");
        }
    }
}
