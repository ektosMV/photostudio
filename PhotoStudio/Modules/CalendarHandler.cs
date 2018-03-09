using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;

namespace PhotoStudio.Modules
{
    public class CalendarHandler : GoogleCalendarApi
    {
        public string CalendarId { get; set; }
        public CalendarHandler(string appName, string filePath, string calendarId) : base(appName, filePath)
        {
            this.CalendarId = calendarId;
        }

        public EventsResource.ListRequest GetEventRequest(DateTime timeMin, DateTime timeMax)
        {
            var request = service.Events.List(CalendarId);
            request.TimeMin = timeMin;
            request.ShowDeleted = false;
            request.SingleEvents = true;
            request.TimeMax = timeMax;
            request.OrderBy = EventsResource.ListRequest.OrderByEnum.StartTime;
            return request;
        }

        public EventsResource.ListRequest GetEventRequest(DateTime timeMin)
        {
            EventsResource.ListRequest request = service.Events.List(CalendarId);
            request.TimeMin = timeMin;
            request.ShowDeleted = false;
            request.SingleEvents = true;
            request.OrderBy = EventsResource.ListRequest.OrderByEnum.StartTime;
            return request;
        }

        public void AddEventRequest(string summary, DateTime startDateTime, DateTime endDateTime, string timeZone)
        {
            var gevent = new Event
            {
                Summary = summary,
                Start = new EventDateTime
                {
                    DateTime = startDateTime,
                    TimeZone = timeZone
                },
                End = new EventDateTime
                {
                    DateTime = endDateTime,
                    TimeZone = timeZone
                }
            };
            service.Events.Insert(gevent, CalendarId).Execute();
        }


    }
}
