using System;
using System.Linq;
using System.Net;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Pos.Api.ViewModel.Base;
using Pos.BusinessLogic.Dto.Base;
using Pos.Core.Enum;
using Pos.Utility;

namespace Pos.Api.Infrastructure
{
    public class BaseApiController : ControllerBase
    {
        protected internal IActionResult Result(Result resultDto, HttpStatusCode? statusCode = null)
        {
            if (statusCode == null)
                statusCode = resultDto.IsSuccess ? HttpStatusCode.OK : HttpStatusCode.BadRequest;

            return new ObjectResult(new Result{ Messages = resultDto.Messages}) {StatusCode = statusCode.ToInt()};
        }

        protected internal IActionResult Result(BaseResponse baseResponse, HttpStatusCode? statusCode = null)
        {
            if (statusCode == null)
                statusCode = baseResponse.IsSuccess ? HttpStatusCode.OK : HttpStatusCode.BadRequest;

            return new ObjectResult(baseResponse) {StatusCode = statusCode.ToInt()};
        }

        protected internal IActionResult Result(object model, string userMessage = null, ENotificationType status = ENotificationType.Info, HttpStatusCode? statusCode = null)
        {
            if (model is BaseResponse baseResponse) 
                return Result(baseResponse, statusCode);

            if (model is Result result) 
                return Result(result, statusCode);

            var response = new BaseResponse {Data = model};
            if (!string.IsNullOrEmpty(userMessage))
                response.Messages.Add(
                    new Notification
                    {
                        Message = userMessage,
                        NotificationType = status
                    }
                );
            if (statusCode == null)
                statusCode = status == ENotificationType.Error ? HttpStatusCode.BadRequest : HttpStatusCode.OK; 

            return new ObjectResult(response){StatusCode = statusCode.ToInt()};
        }

        protected Guid GetUserId()
        {
            Guid.TryParse(User.Claims.First(i => i.Type == ClaimTypes.Sid).Value, out var userId);
            return userId;
        }
    }
}
