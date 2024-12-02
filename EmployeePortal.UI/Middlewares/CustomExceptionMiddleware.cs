using Azure;
using EmployeePortal.Application.Exceptions;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using NPOI.SS.Formula.Functions;
using System.Net;

namespace EmployeePortal.UI.Middlewares
{
    public class CustomExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<CustomExceptionMiddleware> _logger;

        public CustomExceptionMiddleware(RequestDelegate next, ILogger<CustomExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unhandled exception occurred.");
                await HandleExceptionAsync(context, ex);
            }

            // Handle specific status codes (like 404)
            if (context.Response.StatusCode == 404)
            {
                await HandleStatusCodeAsync(context, 404);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var customException = exception as BaseException;
            var statusCode = (int)HttpStatusCode.InternalServerError;
            var message = "An unexpected error occurred.";
            var description = $"{exception.Message}. Contact administrator.";


            // Custom exceptions with specific details
            if (customException != null)
            {
                message = customException.Message;
                description = customException.Description;
                statusCode = customException.Code;
            }

            // Set the response status code
            context.Response.StatusCode = statusCode;
            context.Response.ContentType = "application/json";
            return context.Response.WriteAsync(JsonConvert.SerializeObject(new 
            {
                statusCode,
                message,
                description
            }));
        }

        private static Task HandleStatusCodeAsync(HttpContext context, int statusCode)
        {
            context.Response.Redirect($"/Error/{statusCode}");
            return Task.CompletedTask;
        }
    }

}
