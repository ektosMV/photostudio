using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace BookingDB.DataTypes
{
    public class BookedEntity
    {
        [Key]
        public int BookingEntityId { get; set; }
        public string EntityName { get; set; }
        public int TimeShift { get; set; }
    }
}
