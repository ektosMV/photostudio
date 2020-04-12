using BookingDB;
using BookingDB.Models;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using Remotion.Linq.Clauses;
using Xunit;

namespace xUnitTestApp.Tests
{
    public class BookingDbFixture : IDisposable
    {
        public DbContextOptions DbContextOptions { get; private set; }
        private SqliteConnection _inMemorySqlite;
        public BookingDbFixture()
        {
            _inMemorySqlite = new SqliteConnection("Data Source=:memory:");
            _inMemorySqlite.Open();
            //Turns out the UseSqllite takes in DbConnection, and this meant 
            //I can just store it in the startup so it remains open.
            DbContextOptions = new DbContextOptionsBuilder<BookingDbContext>().
                UseSqlite(_inMemorySqlite).Options;
            using (var context = new BookingDbContext(DbContextOptions))
            {
                context.Database.EnsureCreated();
            }
            AddInitialDataToDb();
        }


        private void AddInitialDataToDb()
        {
            using (var context = new BookingDbContext(DbContextOptions))
            {
                context.BookedEntity.Add(new BookedEntity()
                {
                    EntityName = "TestEntity1",
                    TimeShift = 300
                });
                context.Customer.Add(new Customer()
                {
                    Name = "TestCustomer"
                });
                context.SaveChanges();
                context.Booking.AddRange(new Booking()
                    {
                        DateFrom = DateTime.Now.AddHours(-2),
                        DateTo = DateTime.Now.AddHours(-1),
                        Comment = "TestBookingComment 1",
                        Customer = context.Customer.First(),
                        BookedEntity = context.BookedEntity.First()
                    }, new Booking()
                    {
                        DateFrom = DateTime.Now.AddHours(-3),
                        DateTo = DateTime.Now.AddHours(-2),
                        Comment = "TestBookingComment 2",
                        Customer = context.Customer.First(),
                        BookedEntity = context.BookedEntity.First()
                    },
                    new Booking()
                    {
                        DateFrom = DateTime.Now.AddHours(-5),
                        DateTo = DateTime.Now.AddHours(-4),
                        Comment = "TestBookingComment 3",
                        Customer = context.Customer.First(),
                        BookedEntity = context.BookedEntity.First()
                    });
                context.SaveChanges();
            }
            
        }

        
        public void Dispose()
        {
            _inMemorySqlite.Close();
        }
    }
    
    public class BookingDbContextTests : IClassFixture<BookingDbFixture>
    {
        private BookingDbFixture _bookingDbFixture { get; set; }

        public BookingDbContextTests(BookingDbFixture bookingDbFixture)
        {
            _bookingDbFixture = bookingDbFixture;
        }
        [Fact]
        public void Add_writes_to_database()
        {
            using (var context = new BookingDbContext(_bookingDbFixture.DbContextOptions))
            {
                var entity = context.Booking.Select(x => x);
                var e = context.Customer.Select(x => x);
                var ee = entity.Where(x => x.Customer == e.First());
            }
        }

        [Fact]
        public void TryToAddOverlapTime()
        {
            using (var context = new BookingDbContext(_bookingDbFixture.DbContextOptions))
            {
                context.Booking.Add(new Booking()
                {
                    BookedEntity = context.BookedEntity.First(x => !x.SimultaniousBookings),
                    Comment = "OverlapTime entity",
                    DateFrom = DateTime.Now.AddMinutes(-90),
                    DateTo = DateTime.Now.AddMinutes(-30)
                });
                context.Booking.Add(new Booking()
                {
                    BookedEntity = context.BookedEntity.First(x => !x.SimultaniousBookings),
                    Comment = "OverlapTime entity",
                    DateFrom = DateTime.Now.AddMinutes(-190),
                    DateTo = DateTime.Now.AddMinutes(30)
                });
                context.SaveChanges();
            }
        }

        [Fact]
        public void CreateBokingDbContextFromConfig()
        {
            using (var context = new BookingDbContext())
            {
                context.Database.EnsureCreated();
                AddInitialDataToDb(context);
            }
        }

        void AddInitialDataToDb(BookingDbContext context)
        {
            context.BookedEntity.Add(new BookedEntity()
            {
                EntityName = "TestEntity1",
                TimeShift = 300
            });
            context.Customer.Add(new Customer()
            {
                Name = "TestCustomer"
            });
            context.SaveChanges();
            context.Booking.AddRange(new Booking()
            {
                DateFrom = DateTime.Now.AddHours(-2),
                DateTo = DateTime.Now.AddHours(-1),
                Comment = "TestBookingComment 1",
                Customer = context.Customer.First(),
                BookedEntity = context.BookedEntity.First()
            }, new Booking()
            {
                DateFrom = DateTime.Now.AddHours(-3),
                DateTo = DateTime.Now.AddHours(-2),
                Comment = "TestBookingComment 2",
                Customer = context.Customer.First(),
                BookedEntity = context.BookedEntity.First()
            },
                new Booking()
                {
                    DateFrom = DateTime.Now.AddHours(-5),
                    DateTo = DateTime.Now.AddHours(-4),
                    Comment = "TestBookingComment 3",
                    Customer = context.Customer.First(),
                    BookedEntity = context.BookedEntity.First()
                });
            context.SaveChanges();

        }

    }
}
