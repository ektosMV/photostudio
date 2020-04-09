

using System;

namespace BookingDB.Models
{
    public class BookedEntityAvailableDate
    {
        public BookedEntity BookedEntity { get; set; }
        public DateTime DateTimeFrom { get; set; }
        public DateTime DateTimeTo { get; set; }
    }
}
