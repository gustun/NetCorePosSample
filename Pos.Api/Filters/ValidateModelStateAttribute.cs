using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Pos.BusinessLogic.Dto.Base;

namespace Pos.Api.Filters
{
    public class ValidateModelStateAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext actionContext)
        {
            if (actionContext.ModelState.IsValid) return;
            var result = new Result();

            foreach (var modelState in actionContext.ModelState.Values)
                foreach (var error in modelState.Errors)
                    result.AddError(error.ErrorMessage);    

            actionContext.Result = new BadRequestObjectResult(result);
        }
    }
}
