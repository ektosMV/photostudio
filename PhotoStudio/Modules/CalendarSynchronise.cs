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
    public class CalendarSynchronise : CalendarHandler
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

        
        
        public void AddNewBookingsFromGoogleCaledar()
        {
            var googleCalendarEvents = GetEventRequest(DateTime.Now);
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
            var r = GetEventsFromLocalBase().ToList();
        }


    }
}
