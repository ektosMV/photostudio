using System;
using System.Collections.Generic;
using System.Linq;

namespace PhotoStudio.Models.Booking
{
    public class BookingService : IBookingService
    {
        private BookingContext _bookingContext;

        public BookingService(BookingContext context)
        {
            _bookingContext = context;
        }

        public List<BookingModel> GetAllBookings()
        {
            return _bookingContext.Bookings.ToList();
           //return (from book in _bookingContext.Bookings select book).ToList();
        }

        public List<BookingModel> GetBookingsFromStartTime(DateTime startTime)
        {
            return _bookingContext.Bookings.Where(x => x.TimeOfVisit >= startTime).ToList();
            //return (from book in _bookingContext.Bookings where book.TimeOfVisit >= startTime select book).ToList();
        }

        public List<BookingModel> GetBookingsBetweenTimes(DateTime startTime, DateTime endTimeOfVisitTime)
        {
            return (from book in _bookingContext.Bookings where book.TimeOfVisit >= startTime && book.TimeOfVisit <= endTimeOfVisitTime select book).ToList();
        }

        public List<BookingModel> GetBookingsByCustomerId(int id)
        {
            return _bookingContext.Bookings.Where(x => x.Customer.CustomerId == id).ToList();
        }

        public void AddBooking(BookingModel bookingModel)
        {
            //_bookingContext.Customers.Add(bookingModel.Customer);
            
            _bookingContext.Bookings.Add(new BookingModel(){BookingId = bookingModel.BookingId, TimeOfVisit = bookingModel.TimeOfVisit, Customer = new Customer(){CustomerId = bookingModel.Customer.CustomerId, CustomerPhone =  bookingModel.Customer.CustomerPhone, CustomerName = bookingModel.Customer.CustomerName, CustomerEmail = bookingModel.Customer.CustomerEmail}, EndTime = bookingModel.EndTime});
            _bookingContext.SaveChanges();
        }

        
    }

}
