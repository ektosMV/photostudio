using System;
using System.Collections.Generic;
using System.Text;
using PhotoStudio.Modules.CalendarGenerator;
using Xunit;

namespace xUnitTestApp.Tests
{
    public class GetCalendarTest
    {
        [Fact]
        public void GetCalendarGenerator()
        {
            var c = new CalendarGenerator("ru",5);
            var result = c.GetCalendarGrid(4);
            Assert.Equal(result.Count, 7*4);
            result = c.GetCalendarGrid(4, 1);
            Assert.Equal(result.Count, 7 * 4);
        }
        
    }
}
