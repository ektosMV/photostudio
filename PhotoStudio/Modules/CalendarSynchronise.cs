using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using PhotoStudio.Models;

namespace PhotoStudio.Modules
{
    public class CalendarSynchronise : CalendarHandler
    {
        private BookingContext db;
        
        public CalendarSynchronise(string appName, string filePath, string calendarId, BookingContext context) : base(appName, filePath, calendarId)
        {
            db = context;
        }

        public void getEventsFromLocalBase()
        {
            var queryable = db.Bookings.Select(x => x.TimeOfVisit > DateTime.Now);
        }

    }
}
