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
        public DbSet<BookingModel> Bookings { get; set; }
        public DbSet<Customer> Customers { get; set; }

        public BookingContext(DbContextOptions<BookingContext> options) : base(options)
        {
            
        }
    }
}
