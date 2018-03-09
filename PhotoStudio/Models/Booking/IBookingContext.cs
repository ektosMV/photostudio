using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PhotoStudio.Models.Booking;

namespace PhotoStudio.Models
{
    public interface IBookingContext
    {
        DbSet<BookingModel> Bookings { get; set; }
        DbSet<Customer> Customers { get; set; }
        IEnumerable<BookingModel> GetAllBookings();
    }
}
