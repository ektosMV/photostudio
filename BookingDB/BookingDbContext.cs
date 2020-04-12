using BookingDB.Connection;
using BookingDB.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Threading;
using NLog.Fluent;

namespace BookingDB
{
    public sealed class BookingDbContext : DbContext
    {
        private string configFilePath = "DBConfig.json";
        private DbSettings dbSettings;
        public DbSet<Customer> Customer { get; set; }
        public DbSet<BookedEntity> BookedEntity { get; set; }
        public DbSet<Booking> Booking { get; set; }
        
        public BookingDbContext()
        {
            using (var fileStream = File.OpenRead(configFilePath))
            {
                dbSettings = new DataContractJsonSerializer(typeof(DbSettings)).ReadObject(fileStream) as DbSettings;
            }
        }

        public BookingDbContext(DbContextOptions options) : base(options)
        {
            
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //тут всякая магия по настройке свойств моделей (fluent API)a
            modelBuilder.Entity<Booking>().HasIndex(x => new {x.BookingId, x.DateFrom, x.DateTo});
        }


        protected override void OnConfiguring(DbContextOptionsBuilder dbContextOptionsBuilder)
        {
            if (!dbContextOptionsBuilder.IsConfigured)
            {
                switch (Enum.Parse(typeof(DbTypes), dbSettings.PreferedProvider, true))
                {
                    case DbTypes.MySQL:
                        dbContextOptionsBuilder.UseMySQL(ExtractConnectionString());
                        break;
                   case DbTypes.SqlLite:
                       dbContextOptionsBuilder.UseSqlite(ExtractConnectionString());
                        break;
                    default:
                        throw new SettingsPropertyNotFoundException("");
                }
                //dbContextOptionsBuilder.UseLoggerFactory(LoggerFactory.Create(builder => builder.AddNLog()));
            }
        }

        public string ExtractConnectionString()
        {
            var cs = dbSettings.ConnectionStrings
                .SingleOrDefault(x => x.Key.Equals(dbSettings.PreferedProvider, StringComparison.OrdinalIgnoreCase)).Value;
            if (cs==null)
                throw new Exception("Unable to extract the connection string from the config");
            return cs;
        }

        public override int SaveChanges()
        {
            try
            {
                ValidateBooking();
                return base.SaveChanges();
            }
            catch (ValidationException ex)
            {
                return 0;
            }
            catch (DbUpdateException)
            {
                return 0;
            }
        }

        private ValidationResult ValidateBooking()
        {
            var changedEntities = ChangeTracker.Entries<Booking>()
                .Where(_ => _.State == EntityState.Added ||
                            _.State == EntityState.Modified);
            //var errors = new List<ValidationResult>(); // all errors are here
            foreach (var e in changedEntities)
            {
                if  (e.Entity.BookedEntity.SimultaniousBookings)
                    continue;
                if (Booking.Any(x => 
                e.Entity.DateFrom < x.DateTo && e.Entity.DateTo > x.DateFrom ))
                    return new ValidationResult("Booking time overlap have been detected, " +
                                                  "ensure that entered time is not intersected with existing", new List<string>());
                /* errors.Add(new ValidationResult("Booking time overlap have been detected, " +
                                                     "ensure that entered time is not intersected with existing"));*/
                e.State = EntityState.Detached;
                
                var vc = new ValidationContext(e.Entity);
                //var b = Validator.TryValidateObject(
                //    e.Entity, vc, errors, validateAllProperties: true);*/
            }
            return null;
        }

    }
}
