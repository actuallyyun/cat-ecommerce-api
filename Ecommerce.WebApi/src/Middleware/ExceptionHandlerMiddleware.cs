using Microsoft.EntityFrameworkCore;
using System.Net;
using Ecommerce.Core.src.Common;

namespace Ecommerce.WebApi.src.middleware
{
    public class ExceptionHandlerMiddleware : IMiddleware
    {
        private readonly ILogger<ExceptionHandlerMiddleware> _logger;

        public ExceptionHandlerMiddleware(ILogger<ExceptionHandlerMiddleware> logger)
        {
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (DbUpdateException dbEx)
            {
                _logger.LogError(dbEx, "Database update error occurred.");

                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                var errorMessage = new { StatusCode = context.Response.StatusCode, Message = "A database error occurred. Please check your input or try again later." };
                await context.Response.WriteAsJsonAsync(errorMessage);
            }
            catch (AppException appEx)
            {
                _logger.LogError(appEx, $"Application-specific error: {appEx.ErrorMessage}");

                context.Response.StatusCode = (int)appEx.StatusCode;
                var errorMessage = new { StatusCode = context.Response.StatusCode, Message = appEx.ErrorMessage };
                await context.Response.WriteAsJsonAsync(errorMessage);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred.");

                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                var errorMessage = new { StatusCode = context.Response.StatusCode, Message = "An unexpected error occurred. Please try again later." };
                await context.Response.WriteAsJsonAsync(errorMessage);
            }
        }
    }
}
