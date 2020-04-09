using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BookingDB.Models
{
    /// <summary>
    /// An entity which is being booked
    /// </summary>
    public class BookedEntity
    {
        [Key]
        public int BookingEntityId { get; set; }
        public string EntityName { get; set; }
        public int TimeShift { get; set; }
        public bool SimultaniousBookings { get; set; }
        //navigation properties 
        public List<Booking> Bookings { get; set; }
    }
}
