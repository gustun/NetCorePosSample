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

            foreach (var key in actionContext.ModelState.Keys)
            {
                var modelState = actionContext.ModelState[key];
                foreach (var error in modelState.Errors)
                {
                    var errorDetail = error.ErrorMessage;
                    if (string.IsNullOrEmpty(errorDetail))
                    {
                        var splittedPrpName = key.Split('.');
                        var prpName = splittedPrpName.Length > 1 ? splittedPrpName[1] : key;
                        errorDetail = prpName + " is not valid!";
                    }
                    result.AddError(errorDetail);
                }
            }

            actionContext.Result = new BadRequestObjectResult(result);
        }
    }
}
