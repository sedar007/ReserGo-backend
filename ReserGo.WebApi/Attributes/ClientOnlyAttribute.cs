using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ReserGo.Common.Enum;
using ReserGo.Shared.Interfaces;

namespace ReserGo.WebAPI.Attributes;
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
public class ClientOnlyAttribute : Attribute, IAuthorizationFilter {
    public void OnAuthorization(AuthorizationFilterContext context) {
        var securityService = context.HttpContext.RequestServices.GetService(typeof(ISecurity)) as ISecurity;
        if (securityService is null) {
            context.Result = new UnauthorizedObjectResult("Security service not available");
            return;
        }

        var user = securityService.GetCurrentUser();
        if (user is null || user.Role != UserRole.Client)
            context.Result =
                new UnauthorizedObjectResult(
                    "Access restricted. Please ensure you are logged in and have the correct permissions.");
    }
}