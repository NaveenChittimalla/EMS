using EMS.WebApi.EfCore.ObjectResults;
using Microsoft.AspNetCore.Mvc.Filters;

namespace EMS.WebApi.EfCore.Filters;

/// <summary>
/// Use this filter or decorate your api controller with [ApiController] attribute which automatically does this. 
/// </summary>
public class ModelValidationActionFilter : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        if (!context.ModelState.IsValid
            && context.HttpContext.Request.Method.ToLower() is "post" or "put")
        {
            context.Result = new BadRequestProblemValidationObjectResult(context.ModelState);
        }
    }
}
