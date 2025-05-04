using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ReserGo.Common.Enum;
using ReserGo.Shared.Interfaces;

namespace ReserGo.WebAPI.Attributes;

public class AdminOnlyAttribute : Attribute, IAuthorizationFilter {
    public void OnAuthorization(AuthorizationFilterContext context) {
        var securityService = context.HttpContext.RequestServices.GetService(typeof(ISecurity)) as ISecurity;
        var user = securityService.GetCurrentUser();
        if (user is null || user.Role != UserRole.Admin) {
            context.Result = new UnauthorizedObjectResult("Admin access required");
        }
    }
}