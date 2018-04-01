using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using PhotoStudio.Models;
using PhotoStudio.Models.Booking;
using PhotoStudio.Modules;
using PhotoStudio.Modules.CalendarGenerator;
using IConfiguration = Castle.Core.Configuration.IConfiguration;

namespace PhotoStudio.Controllers
{
    public class BookingController : Controller
    {
        private BookingContext db;
        private CalendarSynchronise CalendarSynchronise { get; set; }
        private CalendarGenerator CalendarGenerator { get; set; }
        private IBookingService BookingService { get; set; }
        private int WeeksToDisplay { get; }
        public int WeekDelta { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="bookingService"></param>
        public BookingController(BookingContext context, IBookingService bookingService, IOptions<GoogleConfiguration> googleConfiguration, IOptions<CalendarGeneratorConfiguration> calendarGenConfig)
        {
            db = context;
            BookingService = bookingService;
            CalendarSynchronise = new CalendarSynchronise(db);
            var r = googleConfiguration.Value.CalendarId;
            var c = googleConfiguration.Value.AppName;
            CalendarGenerator = new CalendarGenerator(calendarGenConfig.Value.Language, calendarGenConfig.Value.UtcDelta);
            WeeksToDisplay = calendarGenConfig.Value.WeeksToDisplay;
            WeekDelta = 0;
        }
   

        public IActionResult GetBookedData()
        {
            var r = CalendarSynchronise.GetEventsFromLocalBase();
            var calendarGrid = CalendarGenerator.GetCalendarGrid(4);
            return View(calendarGrid);
        }

        public IActionResult GetBookingInfoForCustomers()
        {
         /*   var dateTimes = db.Bookings.Select(x => x.TimeOfVisit)
                .Where(x => x.Date > DateTime.Now && x.Date < DateTime.Now.AddMonths(1));*/
            CalendarSynchronise.GetEventsFromLocalBase();
          //  return service.GetService<CalendarSynchronise>().GetEventsFromLocalBase();
            ViewData["3"] = "rt";
            return View();
        }

        public IActionResult GetCalendar()
        {
            var calendar = new CalendarGenerator("ru",5);
            return View();
        }
    }
}
