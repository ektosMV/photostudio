using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PhotoStudio.Models.Booking
{
    public class BookingModel
    {
        [Key]
        public int BookingId { get; set; }
        public DateTime TimeOfVisit { get; set; }
        public DateTime EndTime { get; set; }
        public Customer Customer { get; set; }
    }
}
