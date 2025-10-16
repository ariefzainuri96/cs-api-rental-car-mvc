using System;
using System.Collections.Generic;

namespace cs_api_rental_car_mvc.Dtos.Response
{
    public class PaginationBaseResponse<T> where T : class
    {
        public List<T> Items { get; set; } = new List<T>();
        public int TotalCount { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
    }
}