using System;

namespace cs_api_rental_car_mvc.Dtos.Response
{
    public class BaseResponse<T> where T : class
    {
        public int Status { get; set; }
        public string Message { get; set; } = string.Empty;
        public T? Data { get; set; }
    }
}