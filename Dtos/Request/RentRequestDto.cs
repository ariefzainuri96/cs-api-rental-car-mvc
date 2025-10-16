using System;

namespace cs_api_rental_car_mvc.Dtos.Request
{
    public class RentRequestDto
    {
        public int CarId { get; set; }
        public int UserId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}