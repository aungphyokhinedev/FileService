

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using AplusExtension;
namespace AuthenticationService;
public class Allowed: TypeFilterAttribute {
    
    public Allowed(string roles): base(typeof(AuthorizeAction)) {
        Arguments = new object[] {
            roles
        };
    }
}

public class AuthorizeAction: IAsyncAuthorizationFilter {
    private readonly string _roles;
    private IAuthenticate _auth;
    public AuthorizeAction(IAuthenticate auth,string roles) {
        _roles = roles;
        _auth = auth;
    }
  
    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        try
        {
            var token = context.HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var claim = await _auth.validateTokenAsync(token);
            context.HttpContext.setClaim(claim);

            if(_roles.Trim() != "*"){
                var roles =  _roles.Split(",");
                if(!roles.Contains(claim.role)) {
                    context.Result = new JsonResult(new { message = "Unauthorized role" }) { StatusCode = StatusCodes.Status401Unauthorized };
                }
            }
            
            
        }
        catch (Exception ex)
        {
            context.Result = new JsonResult(new { message = "Unauthorized" }) { StatusCode = StatusCodes.Status401Unauthorized };
        }
    }
}