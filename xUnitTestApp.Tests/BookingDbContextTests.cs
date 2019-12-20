using System;
using System.Collections.Generic;
using System.Text;
using BookingDB;
using BookingDB.DataTypes;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace xUnitTestApp.Tests
{
    public class BookingDbContextTests
    {
        [Fact]
        public void Add_writes_to_database()
        {
            var options = new DbContextOptionsBuilder<BookingDbContext>()
                .UseInMemoryDatabase(databaseName: "Add_writes_to_database")
                .Options;

            // Run the test against one instance of the context
            using (var context = new BookingDbContext(options))
            {
                context.Booking.Add(new Booking()
                {
                    Comment = "Test",
                    Customer = new Customer()
                        {Email = "lal", Name = "lalka", Phone = "1488"}, BookedEntity =  new BookedEntity(){EntityName = "TestEntity"}
                });

            }

        }
    }
}
