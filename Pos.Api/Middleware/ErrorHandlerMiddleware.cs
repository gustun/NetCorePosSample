using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Pos.Api.ViewModel.Base;
using Pos.BusinessLogic.Dto.Base;
using Pos.Contracts;
using Pos.Core.Enum;
using Pos.Utility;

namespace Pos.Api.Middleware
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILoggerHelper _logger;

        public ErrorHandlerMiddleware(RequestDelegate next, ILoggerHelper logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            var errorGuid = Guid.NewGuid();
            var errorLog = $"ErroId: {errorGuid}; ExceptionMessage: {ex.Message}; StackTrace: {ex.StackTrace};";
            _logger.LogError(errorLog);

            var exceptionResponse = new ErrorResponse
            {
                ErrorTraceId = errorGuid.ToString(),
                Messages = new List<Notification>
                {
                    new Notification {Message = "Unexpected error occurred.", NotificationType = ENotificationType.Error}
                }
            };

            context.Response.StatusCode = HttpStatusCode.InternalServerError.ToInt();
            context.Response.ContentType = "application/json";
            return context.Response.WriteAsync(JsonConvert.SerializeObject(exceptionResponse));
        }
    }
}
