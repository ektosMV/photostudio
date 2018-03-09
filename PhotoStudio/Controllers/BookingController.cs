using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PhotoStudio.Models;

namespace PhotoStudio.Controllers
{
    public class BookingController : Controller
    {
        private BookingContext db;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public BookingController(BookingContext context) => db = context;

        public IActionResult GetBookedData()
        {
            return View(db.GetAllBookings());
        }

        public IActionResult GetBookingInfoForCustomers()
        {
            var dateTimes = db.Bookings.Select(x => x.TimeOfVisit)
                .Where(x => x.Date > DateTime.Now && x.Date < DateTime.Now.AddMonths(1));

            return View();
        }
    }
}
