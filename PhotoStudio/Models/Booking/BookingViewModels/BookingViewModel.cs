using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PhotoStudio.Modules.CalendarGenerator;

namespace PhotoStudio.Models.Booking.BookingViewModels
{
    public class BookingViewModel
    {
        public List<string> DaysOfWeek { get; set; }
        public List<BookingModel> BookingModels { get; set; }
        public List<CalendarData> CalendarData { get; set; }
        //public List<CalendarDataViewModel> CalendarDataViewModels { get; set; }
    }

   /* public class CalendarDataViewModel
    {
        public CalendarData CalendarData { get; set; }
        public List<BookingModel> BookingModels { get; set; }
    }*/
}
