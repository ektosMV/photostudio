using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Moq;
using PhotoStudio.Controllers;
using PhotoStudio.Models;
using Xunit;
using PhotoStudio.Models.Booking;
using StackExchange.Redis;
using Microsoft.Extensions.Configuration;

namespace xUnitTestApp.Tests
{

    public class LocalBaseTests
    {
        public IConfiguration Configuration { get; }
        [Fact]
        public void GetListOfBookings()
        {
            var bookingdata = GenerateTestBookingData().AsQueryable();
            var mockSet = new Mock<DbSet<BookingModel>>();
            mockSet.As<IQueryable<BookingModel>>().Setup(m => m.Provider).Returns(bookingdata.Provider);
            mockSet.As<IQueryable<BookingModel>>().Setup(m => m.Expression).Returns(bookingdata.Expression);
            mockSet.As<IQueryable<BookingModel>>().Setup(m => m.ElementType).Returns(bookingdata.ElementType);
            mockSet.As<IQueryable<BookingModel>>().Setup(m => m.GetEnumerator()).Returns(bookingdata.GetEnumerator());

           // string connection = Configuration.GetConnectionString("Server=(localdb)\\mssqllocaldb;Database=bookingsstoredb;Trusted_Connection=True;MultipleActiveResultSets=true");
           
            var contextOptions = new DbContextOptions<BookingContext>();
            var mockContext = new Mock<BookingContext>(contextOptions);
            mockContext.Setup(c => c.Bookings).Returns(mockSet.Object);

            var service = new BookingService(mockContext.Object);
            var bookings = service.GetAllBooks();
            var r = mockContext.Object.GetAllBookings();
            var rr = mockContext;
            // mockContext.As<IQueryable<BookingModel>>().Setup(m => m.GetEnumerator()).Returns(mockContext());

            //  var r = mockContext.Object.GetAllBookings();

        }

        public List<BookingModel> GenerateTestBookingData()
        {
            var cust = GenerateCustomerData();
            var bookings = new BookingModel[3]
            {
                new BookingModel()
                {
                    BookingId = 0,
                    Customer = cust[0],
                    TimeOfVisit = DateTime.Now.AddHours(1),
                    EndTime = DateTime.Now.AddHours(2)
                },
                new BookingModel()
                {
                    BookingId = 1,
                    Customer = cust[1],
                    TimeOfVisit = DateTime.Now.AddHours(2),
                    EndTime = DateTime.Now.AddHours(3)
                },
                new BookingModel()
                {
                    BookingId = 2,
                    Customer = cust[2],
                    TimeOfVisit = DateTime.Now.AddHours(3),
                    EndTime = DateTime.Now.AddHours(4)
                }
            }.AsQueryable();
            return bookings.ToList();
        }

        public List<Customer> GenerateCustomerData()
        {
            var cust = new Customer[3]
            {
                new Customer()
                {
                    CustomerId = 0,
                    CustomerEmail = "test1@t1.ru",
                    CustomerName = "Test1",
                    CustomerPhone = "01"
                },
                new Customer()
                {
                    CustomerId = 1,
                    CustomerEmail = "test2@t2.ru",
                    CustomerName = "Test2",
                    CustomerPhone = "02"
                },
                new Customer()
                {
                    CustomerId = 2,
                    CustomerEmail = "test3@t1.ru",
                    CustomerName = "Test3",
                    CustomerPhone = "03"
                }
            }.AsQueryable();
            return cust.ToList();
        }
    }

}

