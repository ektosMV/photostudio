using System;
using System.Linq;
using Xunit;
using PhotoStudio;
using PhotoStudio.Modules;

namespace xUnitTestApp.Tests
{
    public class GoogleCalendarHandlerTest
    {
        private CalendarHandler _calendarHandler;
        public GoogleCalendarHandlerTest()
        {
            //_calendarHandler = new CalendarHandler("TestApp", @"..\..\..\..\PhotoStudio\Modules\client_secret.json", "ohom3k7j2c9q0pl3jm0hcslu08@group.calendar.google.com");
        }
        [Fact]
        public void AddEventRequestTest()
        {
            var starttime = new DateTime(DateTime.Now.Ticks).AddDays(1);
            var endTime = new DateTime(DateTime.Now.Ticks).AddDays(1).AddHours(1);
            _calendarHandler.AddEventRequest("Test", starttime, endTime, "Asia/Yekaterinburg");
            
        }
        [Fact]
        public void DeleteRequestTest()
        {
            _calendarHandler.DeleteAllEvents();
            Assert.Equal(0, GetCountOfEvents());
        }
        
        public int GetCountOfEvents()
        {
            return _calendarHandler.GetEventRequest(DateTime.MinValue).Count;
        }

        [Fact]
        public void dd()
        {
            var l = new LocalBaseTests();

        }
        


    }
}
