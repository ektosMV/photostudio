using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BookingDB.Models
{
    public class BookedEntity
    {
        [Key]
        public int BookingEntityId { get; set; }
        public string EntityName { get; set; }
        public int TimeShift { get; set; }
    }
}
