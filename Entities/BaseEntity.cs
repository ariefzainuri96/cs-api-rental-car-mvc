using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace cs_api_rental_car_mvc.Entities
{
    public class BaseEntity
    {
        [Column("id")]
        public int Id { get; set; }
        
        [Column("created_at")]
        public DateTimeOffset CreatedAt { get; set; }
        
        [Column("updated_at")]
        public DateTimeOffset? UpdatedAt { get; set; }
        
        [Column("deleted_at")]
        public DateTimeOffset? DeletedAt { get; set; }
    }
}