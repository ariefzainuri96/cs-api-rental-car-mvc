using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace cs_api_rental_car_mvc.Dtos.Request
{
    public class RentRequestDto
    {
        [Required(ErrorMessage = "CarId is required")]
        public int CarId { get; set; }
        [Required(ErrorMessage = "StartDate is required")]
        public DateTime StartDate { get; set; }
        [Required(ErrorMessage = "EndDate is required")]
        public DateTime EndDate { get; set; }
        [JsonIgnore]
        public int UserId { get; set; }
    }
}