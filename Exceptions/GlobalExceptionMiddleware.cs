using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace cs_api_rental_car_mvc.Exceptions
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;

        public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context); // continue the pipeline
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            _logger.LogError(exception, "An unexpected error occurred: {Message}", exception.Message);

            HttpStatusCode statusCode;
            string detail;

            if (exception is ArgumentException)
            {
                statusCode = HttpStatusCode.BadRequest;
                detail = exception.Message;
            }
            else if (exception is EntityNotFoundException)
            {
                statusCode = HttpStatusCode.NotFound;
                detail = exception.Message;
            }
            else if (exception is UnauthorizedAccessException)
            {
                statusCode = HttpStatusCode.Forbidden;
                detail = "Access denied.";
            }
            else
            {
                statusCode = HttpStatusCode.InternalServerError;
                detail = "An internal server error occurred. Please try again later.";
            }

            var response = new
            {
                Status = (int)statusCode,
                Message = detail,
            };

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            var json = JsonSerializer.Serialize(response);
            await context.Response.WriteAsync(json);
        }
    }
}