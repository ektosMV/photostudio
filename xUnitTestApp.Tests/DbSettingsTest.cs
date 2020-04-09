using System;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using Xunit;
using BookingDB;
using BookingDB.Connection;
using MySql.Data.MySqlClient;

namespace xUnitTestApp.Tests
{
    public class DbSettingsTest
    {
        private string filePath = "DBConfig.json";
        [Fact]
        public void ReadingConfigTest()
        {
            DbSettings dbSettings;
            Assert.True(File.Exists(filePath));
            using (var fileStream = File.OpenRead(filePath))
            {
                dbSettings = new DataContractJsonSerializer(typeof(DbSettings)).ReadObject(fileStream) as DbSettings;
            }
            Assert.NotNull(dbSettings);
            Assert.NotEmpty(dbSettings.ConnectionStrings);
            Assert.Contains(dbSettings.PreferedProvider, Enum.GetNames(typeof(DbTypes)).ToList());
        }

        [Fact]
        public void OnConfiguringTest()
        {
            var c = new MySqlConnectionStringBuilder(new BookingDbContext().ExtractConnectionString());
           Assert.NotNull(new BookingDbContext().ExtractConnectionString());
        }
    }
}
