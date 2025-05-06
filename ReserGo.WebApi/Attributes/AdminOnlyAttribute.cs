using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ReserGo.Common.Enum;
using ReserGo.Shared.Interfaces;
using ReserGo.Common.Security;

namespace ReserGo.WebAPI.Attributes;

public class AdminOnlyAttribute : Attribute, IAuthorizationFilter {
    public void OnAuthorization(AuthorizationFilterContext context) {
        var securityService = context.HttpContext.RequestServices.GetService(typeof(ISecurity)) as ISecurity;
        if (securityService is null) {
            context.Result = new UnauthorizedObjectResult("Security service not available");
            return;
        }

        var user = securityService.GetCurrentUser();
        if (user is null || user.Role != UserRole.Admin)
            context.Result = new UnauthorizedObjectResult("Admin access required");
    }
}