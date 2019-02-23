using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhotoStudio.Modules.CalendarGenerator
{
    public class CalendarGeneratorConfiguration
    {
        public string Language { get; set; }
        public int UtcDelta { get; set; }
        public int WeeksToDisplay { get; set; }
        public int MaximumWeeksToBooking { get; set; }
    }
}
