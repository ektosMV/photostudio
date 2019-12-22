using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookingDB.Models
{
    
    public class BookedEntitySchedule
    {
        public BookedEntity BookedEntity;
        [Required]
        public TimeSpan TimeFrom { get; set; }
        [Required]
        public TimeSpan TimeTo { get; set; }
        [Required]
        public DayOfWeek DayOfWeek { get; set; }
    }
}