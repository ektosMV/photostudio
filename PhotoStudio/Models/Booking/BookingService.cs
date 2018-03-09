using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Remotion.Linq.Clauses;
using System.Linq;

namespace PhotoStudio.Models.Booking
{
    public class BookingService
    {
        private BookingContext _bookingContext;

        public BookingService(BookingContext context)
        {
            _bookingContext = context;
        }

        public List<BookingModel> GetAllBooks()
        {
            return (from book in _bookingContext.Bookings select book).ToList();
        }
    }

}
