using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhotoStudio.Modules
{
    public interface ICalendarSynchronise
    {
        void SynchroniseCalendar(DateTime startDate);
    }
}
