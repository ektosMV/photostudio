using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PhotoStudio.Models.Booking;

namespace PhotoStudio.Models
{
    public class BookingContext : DbContext
    {
        public virtual DbSet<BookingModel> Bookings { get; set; }
        public virtual DbSet<Customer> Customers { get; set; }

        public BookingContext(DbContextOptions<BookingContext> options) : base(options)
        {
            
        }
    }
}
