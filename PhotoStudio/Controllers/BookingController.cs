﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using PhotoStudio.Models.Booking.BookingViewModels;
using PhotoStudio.Modules.CalendarGenerator;
using System;
using System.IO;

namespace PhotoStudio.Controllers
{
    public class BookingController : Controller
    {

        public IActionResult GetBookedData([FromServices]IOptions<CalendarGeneratorConfiguration> calendarGenConfig)
        {
            var calendarGenerator = new CalendarGenerator(calendarGenConfig.Value.Language, calendarGenConfig.Value.UtcDelta);
            var bvm = new BookingViewModel
            {
                DaysOfWeek = calendarGenerator.GetDays(),
                CalendarData = calendarGenerator.GetCalendarGrid(4)
            };
            return View(bvm);
        }

        public IActionResult CalendarShift([FromServices]IOptions<CalendarGeneratorConfiguration> calendarGenConfig)
        {
            var r = new StreamReader(HttpContext.Request.Body).ReadToEnd();
            var calendarGenerator = new CalendarGenerator(calendarGenConfig.Value.Language, calendarGenConfig.Value.UtcDelta);
            var bvm = new BookingViewModel
            {
                DaysOfWeek = calendarGenerator.GetDays(),
                CalendarData = calendarGenerator.GetCalendarGrid(4, Convert.ToInt16(r))
            };
            var rw = Json(bvm);
            var rrr = new JsonResult(bvm);
            return Json(rrr);
        }


        public void GetAdditionalInfoByDay([FromServices]IOptions<CalendarGeneratorConfiguration> calendarGenConfig)
        {
            var calendarGenerator = new CalendarGenerator(calendarGenConfig.Value.Language, calendarGenConfig.Value.UtcDelta);
            
            var dayPosition = Convert.ToInt16(new StreamReader(HttpContext.Request.Body).ReadToEnd());
            if (dayPosition < 0 || dayPosition > 365)
                return;
            var date = DateTime.UtcNow.AddHours(CalendarGenerator.UtcDelta).Date.AddDays(-DateTime.UtcNow.AddHours(CalendarGenerator.UtcDelta).DayOfWeek.DaysOfWeekFromMonday()).AddDays(dayPosition);
        }

    }
}
