using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace BookingDB.Models
{
    public class Booking
    {
        [Key]
        public int BookingId { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        [MaxLength(5000)]
        public string Comment { get; set; }
        [Required]
        public BookedEntity BookedEntity { get; set; }
        public Customer Customer { get; set; }
    }
}
