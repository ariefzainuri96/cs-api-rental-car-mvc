using System;
using System.ComponentModel.DataAnnotations;

namespace cs_api_rental_car_mvc.Dtos.Request
{
    public class CarRequestDto
    {
        [Required(ErrorMessage = "Brand is required")]
        public string Brand { get; set; } = string.Empty;

        [Required(ErrorMessage = "Model is required")]
        public string Model { get; set; } = string.Empty;

        [Required(ErrorMessage = "Year is required")]
        [Range(1950, int.MaxValue, ErrorMessage = "Year must be greater than 1950")]
        public int Year { get; set; }

        [Required(ErrorMessage = "Plate Number is required")]
        public string PlateNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "Rental Rate Per Day is required")]
        [Range(0, double.MaxValue, ErrorMessage = "Rental Rate Per Day must be greater than 0")]
        public decimal RentalRatePerDay { get; set; }

        public string Status { get; set; } = "Tersedia";
    }
}