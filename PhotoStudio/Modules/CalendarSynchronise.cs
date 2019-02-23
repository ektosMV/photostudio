using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Castle.Core.Internal;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.Configuration;
using PhotoStudio.Models;
using PhotoStudio.Models.Booking;

namespace PhotoStudio.Modules
{
    public class CalendarSynchronise : CalendarHandler, ICalendarSynchronise
    {
        private BookingContext db;
        
        public CalendarSynchronise(BookingContext context)
        {
            db = context;
        }

        public IQueryable<BookingModel> GetEventsFromLocalBase()
        {
            return db.Bookings.Where(x => x.TimeOfVisit > DateTime.Now);
        }

        public IQueryable<BookingModel> GetEventsFromLocalBase(DateTime startDate)
        {
            return db.Bookings.Where(bookings =>
                bookings.TimeOfVisit.Date >= startDate.Date);
        }

        public IQueryable<BookingModel> GetEventsFromLocalBase(DateTime startDate, int weeks)
        {
            return db.Bookings.Where(bookings =>
                bookings.TimeOfVisit.Date >= startDate.Date &&
                bookings.TimeOfVisit.Date <= startDate.AddDays(7 * weeks));
        }

        public List<List<BookingModel>> GetEventsByDays(DateTime startDate, int weeks)
        {
            IOrderedQueryable<BookingModel> dailyBookings;
            var bookings = GetEventsFromLocalBase(startDate, weeks);
           
            var BookingsByDaily = new List<List<BookingModel>>();
            for(int i = 0; i < weeks*7; i++)
            {
                dailyBookings = from booking in bookings
                    where booking.TimeOfVisit.Date == startDate.AddDays(i).Date
                    orderby booking.TimeOfVisit ascending
                    select booking;
                BookingsByDaily.Add(new List<BookingModel>());
                if (dailyBookings.Count() != 0)
                {
                    foreach (var book in dailyBookings)
                    {
                        BookingsByDaily[i].Add(book);
                    }
                }
            }
            return BookingsByDaily;
        }


        public void SynchroniseCalendar(DateTime startDate)
        {
            DeleteOldBookingsFromGoogleCaledar(startDate);
            AddNewBookingsFromGoogleCaledar(startDate);
        }


        private void AddNewBookingsFromGoogleCaledar(DateTime startDate)
        {
            var googleCalendarEvents = GetEventRequest(startDate);
            var localEvents = GetEventsFromLocalBase().ToList();
            var newbookings = new List<BookingModel>();
            foreach (var ev in googleCalendarEvents)
            {
                if (localEvents.Where(x => x.TimeOfVisit == ev.Start.DateTime && x.EndTime == ev.End.DateTime)
                    .IsNullOrEmpty())
                {
                    newbookings.Add(new BookingModel
                    {
                        Customer = null,
                        TimeOfVisit = ev.Start.DateTime.GetValueOrDefault(),
                        EndTime = ev.End.DateTime.GetValueOrDefault(),
                    });
                }
            }
            foreach (var book in newbookings)
            {
                db.Bookings.Add(book);
                db.SaveChanges();
            }
        }

        private void DeleteOldBookingsFromGoogleCaledar(DateTime startDate)
        {
            var googleCalendarEvents = GetEventRequest(startDate);
            var localEvents = GetEventsFromLocalBase(startDate).ToList();
            foreach (var localEvent in localEvents)
            {
                if (googleCalendarEvents
                    .Where(x => x.Start.DateTime == localEvent.TimeOfVisit && x.End.DateTime == localEvent.EndTime)
                    .IsNullOrEmpty())
                {
                    db.Bookings.Remove(localEvent);
                    db.SaveChangesAsync();
                }
            }
        }




    }
}
