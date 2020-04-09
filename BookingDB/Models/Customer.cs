
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BookingDB.Models
{
    public class Customer
    {
        public int CustomerId { get; set; }
        [MaxLength(100)]
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }

        public List<Booking> Bookings { get; set; }
    }
}
