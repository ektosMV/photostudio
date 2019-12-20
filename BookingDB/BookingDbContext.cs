using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using BookingDB.DataTypes;
using Microsoft.EntityFrameworkCore;
using System.Runtime.Serialization.Json;
using BookingDB.Connection;
using Microsoft.Extensions.Logging;
using NLog.Extensions;
using NLog.Extensions.Logging;

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
            

        }


        protected override void OnConfiguring(DbContextOptionsBuilder dbContextOptionsBuilder)
        {
            if (!dbContextOptionsBuilder.IsConfigured)
            {
                switch (Enum.Parse(typeof(DbTypes), dbSettings.PreferedProvider))
                {
                    case DbTypes.MySQL:
                        dbContextOptionsBuilder.UseMySQL(ExtractConnectionString());
                        break;
                    default:
                        throw new SettingsPropertyNotFoundException("");
                }
                dbContextOptionsBuilder.UseLoggerFactory(LoggerFactory.Create(builder => builder.AddNLog()));
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


    }
}
