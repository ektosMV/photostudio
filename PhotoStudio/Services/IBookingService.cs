using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhotoStudio.Models.Booking
{
    public interface IBookingService
    {
        List<BookingModel> GetAllBookings();

        List<BookingModel> GetBookingsFromStartTime(DateTime startTime);

        List<BookingModel> GetBookingsBetweenTimes(DateTime startTime, DateTime endTimeOfVisitTime);

        List<BookingModel> GetBookingsByCustomerId(int id);
        
        void AddBooking(BookingModel bookingModel);
    }
}
