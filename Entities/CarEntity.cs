using System.ComponentModel.DataAnnotations.Schema;

namespace cs_api_rental_car_mvc.Entities
{
    [Table("cars")]
    public class CarEntity: BaseEntity
    {
        [Column("brand")]
        public string Brand { get; set; } = string.Empty;
        [Column("model")]
        public string Model { get; set; } = string.Empty;
        [Column("year")]
        public int Year { get; set; }
        [Column("plate_number")]
        public string PlateNumber { get; set; } = string.Empty;
        [Column("rental_rate_per_day")]
        public decimal RentalRatePerDay { get; set; }
        [Column("status")]
        public string Status { get; set; } = string.Empty;
    }
}