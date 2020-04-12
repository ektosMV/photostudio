using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookingDB;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PhotoStudio.Models;
using PhotoStudio.Models.Booking;
using PhotoStudio.Modules;
using PhotoStudio.Modules.CalendarGenerator;

namespace PhotoStudio
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            string connection = Configuration.GetConnectionString("DefaultConnection");
            //services.AddDbContext<BookingContext>(options => options.UseSqlServer(connection));
            services.AddDbContext<BookingDbContext>();// optionsAction => optionsAction.UseSqlite(""));
            /*services.AddTransient<IBookingService, BookingService>();
            services.AddTransient<CalendarSynchronise>();*/
            /*services.AddTransient<CalendarGenerator>();*/
            
            // Add functionality to inject IOptions<T>
            services.AddOptions();
            // Add our Config object so it can be injected
            var r = Configuration.GetSection("LocalisationSettings");
            //services.Configure<CalendarGeneratorConfiguration>(Configuration.GetSection("LocalisationSettings"));
            services.Configure<GoogleConfiguration>(Configuration.GetSection("googlecalendarsettings"));
            
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();
            
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
                routes.MapRoute(
                    name: "Booking",
                    template: "{controller=Booking}/{action=GetBookedData}/{id?}");
            });
        }
    }
}
