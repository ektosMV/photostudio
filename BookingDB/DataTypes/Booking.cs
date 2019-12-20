using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BookingDB.DataTypes
{
    public class Booking
    {
        [Key]
        public int BookingId { get; set; }
        
        public BookedEntity BookedEntity { get; set; }
        public Customer Customer { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public string Comment { get; set; }


    }
}
