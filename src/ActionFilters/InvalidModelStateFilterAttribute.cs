using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace NamesApi.Filters
{
    public class InvalidModelStateFilterAttribute : ActionFilterAttribute
    {
        private BadRequestObjectResult FormatResult(ModelStateDictionary modelState, string traceId)
        {
                return new BadRequestObjectResult(new 
                {
                    errors = modelState.Where(m => m.Value.Errors.Any()).ToDictionary(
                        kvp => kvp.Key,
                        kvp => kvp.Value.Errors.Select(e => e.ErrorMessage)
                    ),
                    status = StatusCodes.Status400BadRequest,
                    traceId = traceId
                });
        }
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            if(!context.ModelState.IsValid && context.HttpContext.Request.Method == HttpMethod.Patch.Method)
            {
                context.Result = FormatResult(context.ModelState, context.HttpContext.TraceIdentifier);
            }
            base.OnActionExecuted(context);
        }
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if(!context.ModelState.IsValid)
            {
                context.Result = FormatResult(context.ModelState, context.HttpContext.TraceIdentifier);
            }
            else
            {
                base.OnActionExecuting(context);
            }
        }
    }
}