using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.ResponseCaching.Internal;
using Microsoft.Extensions.Configuration;

namespace PhotoStudio.Modules.CalendarGenerator
{
    public class CalendarGenerator
    {
        private string LocalisationCode { get; set; }
        public IConfigurationSection Mounts { get; }
        private IConfigurationSection Days { get; }
        public static int UtcDelta { get; private set; }
        private int WeekDelta { get; set; }

        public static IConfigurationRoot Configuration;
        public CalendarGenerator(string localisationCode, int utcDelta)
        {
            LocalisationCode = localisationCode;
            var builder = new ConfigurationBuilder()
                .AddJsonFile(@"Modules\CalendarGenerator\CalendarGeneratorLocalisation.json")
                .AddEnvironmentVariables();
            Configuration = builder.Build();
            Mounts = Configuration.GetSection(LocalisationCode).GetSection("Month");
            Days = Configuration.GetSection(localisationCode).GetSection("DaysOfWeek");
            UtcDelta = utcDelta;
            WeekDelta = 0;
        }
        public List<CalendarData> GetCalendarGrid(int weeks, int weekdelta=0)
        {
            if (weekdelta < 0)
                weekdelta = 0;
            var lastMonday = DateTime.UtcNow.AddHours(UtcDelta).Date.AddDays(-DeltaToMonday());
            if (weekdelta != 0)
            {
                WeekDelta += weekdelta;
                lastMonday = lastMonday.Date.AddDays(WeekDelta * 7);
            }
            var dateSet = new List<CalendarData>(7*weeks);
            while (dateSet.Count<weeks*7)
            {
                dateSet.Add(new CalendarData{ Date = lastMonday.Day, Month = Mounts[lastMonday.Month.ToString()], Year = lastMonday.Year});
                lastMonday = lastMonday.AddDays(1);
            }
            return dateSet.ToList();
        }

        public List<string> GetDays()
        {
            return Days.GetChildren().Select(x => x.Value).ToList();
        }


        private int DeltaToMonday()
        {
            return (int)DateTime.UtcNow.AddHours(UtcDelta).DayOfWeek.DaysOfWeekFromMonday();
        }
    }

    public static class DateTimeExtension
    {
        public static int DaysOfWeekFromMonday(this DayOfWeek day)
        {
            switch (day)
            {
                case DayOfWeek.Monday: return 0;
                case DayOfWeek.Tuesday: return 1;
                case DayOfWeek.Wednesday: return 2;
                case DayOfWeek.Thursday: return 3;
                case DayOfWeek.Friday: return 4;
                case DayOfWeek.Saturday: return 5;
                case DayOfWeek.Sunday: return 6;
                default:throw new ArgumentOutOfRangeException("Wrong DayOfWeek code");
            }
        }
        
    }
}
