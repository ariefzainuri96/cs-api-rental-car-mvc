using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace cs_api_rental_car_mvc.Dtos
{
    public class HttpError : ObjectResult
    {
        public HttpError(string message, int statusCode = StatusCodes.Status400BadRequest)
            : base(new { error = message, status = statusCode, timestamp = DateTime.UtcNow })
        {
            StatusCode = statusCode;
        }
    }
}