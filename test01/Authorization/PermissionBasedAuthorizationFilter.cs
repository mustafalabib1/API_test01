using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;
using System.Security.Claims;
using test01.Data;
using test01.Repositoies;

namespace test01.Authorization
{

    public class PermissionBasedAuthorizationFilter(ApplicationDbContext dbContext) : IAsyncAuthorizationFilter
    {
        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var attribute = context.ActionDescriptor.EndpointMetadata
                .OfType<CheckPermissionAttribute>()
                .FirstOrDefault();
            if (attribute == null)
            {
                return;
            }
            else
            {
                var claimsIdentity = context.HttpContext.User.Identity as ClaimsIdentity;
                if (claimsIdentity == null || !claimsIdentity.IsAuthenticated)
                {
                    context.Result = new ForbidResult();
                    return;
                }
                else
                {
                    var userIdClaim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
                    if (userIdClaim == null)
                    {
                        context.Result = new ForbidResult();
                        return;
                    }
                    else
                    {
                        var userId = int.Parse(userIdClaim.Value);
                        if (!dbContext.Set<UserPermission>().Any(up=> up.UserId==userId && up.PermissionId == attribute.Permission))
                        {
                            context.Result = new ForbidResult();
                            return;
                        }
                    }
                }
            }
        }
    }
}
