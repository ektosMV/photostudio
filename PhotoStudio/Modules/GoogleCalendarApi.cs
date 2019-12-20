using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Services;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace PhotoStudio.Modules
{
    public class GoogleCalendarApi
    {
        public static IConfigurationRoot Configuration;
        public CalendarService service { get; set; }
        public string CalendarId { get; set; }
        public string AppName { get; set; }

        public GoogleCalendarApi()
        {
            var builder = new ConfigurationBuilder()
                .AddJsonFile(@"Modules\googlecalendarsettings.json")
                .AddEnvironmentVariables();
            Configuration = builder.Build();
            AppName = Configuration.GetSection("GoogleCalendarSettings:AppName").Value;
            CalendarId = Configuration.GetSection("GoogleCalendarSettings:CalendarId").Value;
            service = GetCalendarService();
        }

        public CalendarService GetCalendarService()
        {
            var FilePath = Environment.CurrentDirectory + @"\Modules\client_secret.json";
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
