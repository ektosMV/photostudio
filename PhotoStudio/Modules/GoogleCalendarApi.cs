using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Services;
using Microsoft.CodeAnalysis;

namespace PhotoStudio.Modules
{
    public class GoogleCalendarApi
    {
        public CalendarService service { get; set; }

        public GoogleCalendarApi(string AppName, string FilePath)
        {
            service = GetCalendarService(AppName, FilePath);
        }
        public CalendarService GetCalendarService(string AppName, string FilePath)
        {
            string ApplicationName = AppName;
            GoogleCredential credential;
            using (var stream =
                new FileStream(FilePath, FileMode.Open, FileAccess.Read))
            {
                credential = GoogleCredential.FromStream(stream).CreateScoped(CalendarService.Scope.Calendar);
            }

            // Create Google Calendar API service.
            var service = new CalendarService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });
            return service;
        }
    }
}
