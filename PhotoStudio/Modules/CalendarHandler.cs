﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Http;
using Google.Apis.Util;
using Microsoft.Extensions.Configuration;
using PhotoStudio.Models.Booking;

namespace PhotoStudio.Modules
{
    public class CalendarHandler : GoogleCalendarApi
    {
        public List<Event> GetEventRequest(DateTime timeMin, DateTime timeMax)
        {
            var request = service.Events.List(CalendarId);
            request.TimeMin = timeMin;
            request.ShowDeleted = false;
            request.SingleEvents = true;
            request.TimeMax = timeMax;
            request.OrderBy = EventsResource.ListRequest.OrderByEnum.StartTime;
            return request.Execute().Items.ToList();
        }

        public List<Event> GetEventRequest(DateTime timeMin)
        {
            EventsResource.ListRequest request = service.Events.List(CalendarId);
            request.TimeMin = timeMin;
            request.ShowDeleted = false;
            request.SingleEvents = true;
            request.OrderBy = EventsResource.ListRequest.OrderByEnum.StartTime;
            return request.Execute().Items.ToList();
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
            var result = service.Events.Insert(gevent, CalendarId).Execute();
            if (!result.Status.Equals("confirmed"))
                throw new Exception($"Incorrect status, expected: confirmed, in fact {result.Status}");
        }

        public void DeleteAllEvents()
        {
            var ids = service.Events.List(CalendarId).Execute().Items.Select(x => x.Id).ToList();
            
            var deleteRequestsr = new List<EventsResource.DeleteRequest>();
            foreach (var id in ids)
            {
                deleteRequestsr.Add(new EventsResource.DeleteRequest(service, CalendarId, id));
            }
            foreach (var dr in deleteRequestsr)
            {
                dr.Execute();
            }
         }

            
    }
}
