using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using TokenService;

public class AuthenticateAttribute : ActionFilterAttribute
{
    private IRequestClient<ValidateToken> _validate;

    public AuthenticateAttribute(IRequestClient<ValidateToken> validate)
    {
        _validate = validate;
    }


    public override async Task OnActionExecutionAsync(ActionExecutingContext context,
                                         ActionExecutionDelegate next)
    {
        Console.WriteLine("authentication header:" + context.HttpContext.Request.Headers["Authorization"]);
        var token = context.HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

        var result = await _validate.GetResponse<Payload>(new
        {
            token = token
        });
        // var payload = JwtUtils.ValidateJwtToken(token);
        if (result.Message.response.code == ResultCode.OK)
        {
            // attach user to context on successful jwt validation
            var payload = result.Message.response.payload;

           // var userId = payload["id"];
           // var deviceId = payload["deviceid"];
           // context.HttpContext.Items["meta"] = userId;
           context.HttpContext.Items["__uid"] = payload["id"];
            await next(); 
        }
        else
        {
           // throw new Exception("Unauthorize");
            context.Result = new JsonResult(new { message = "Unauthorized" }) { StatusCode = StatusCodes.Status401Unauthorized };
        }

        // logic before action goes here

        // the actual action

        // logic after the action goes here
    }

}

