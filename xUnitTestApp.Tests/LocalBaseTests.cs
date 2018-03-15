using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Moq;
using PhotoStudio.Models;
using Xunit;
using PhotoStudio.Models.Booking;

namespace xUnitTestApp.Tests
{

    public class LocalBaseTests
    {
        [Fact]
        public void AddBookingTest()
        {
            var bookingdata = GenerateTestBookingData().AsQueryable();
            var mockSet = GetMockset(bookingdata);

            var contextOptions = new DbContextOptions<BookingContext>();
            var mockContext = new Mock<BookingContext>(contextOptions);
            mockContext.Setup(c => c.Bookings).Returns(mockSet.Object);

            var service = new BookingService(mockContext.Object);
            var custId = new List<int>();
            foreach (var book in bookingdata)
            {
                if (!custId.Contains(book.Customer.CustomerId))
                    custId.Add(book.Customer.CustomerId);
            }
            var id = custId[new Random().Next(custId.Count)];
            var cust = new Customer()
            {
                CustomerId = 3,
                CustomerEmail = "test4@t1.ru",
                CustomerName = "TestAdd",
                CustomerPhone = "04"
            };
            var addbooking = new BookingModel()
            {
                BookingId = 4,
                Customer = cust,
                TimeOfVisit = DateTime.Now.AddHours(1),
                EndTime = DateTime.Now.AddHours(2)
            };
            
            service.AddBooking(addbooking);
            mockSet.Verify(m => m.Add(It.IsAny<BookingModel>()), Times.Once());
            mockContext.Verify(m => m.SaveChanges(), Times.Once());
            var res = service.GetAllBookings();
            // Assert.Equal(res, addbooking);
        }


        [Fact]
        public void GetBookingsByCustomerId()
        {
            var bookingdata = GenerateTestBookingData().AsQueryable();
            var mockSet = GetMockset(bookingdata);

            var contextOptions = new DbContextOptions<BookingContext>();
            var mockContext = new Mock<BookingContext>(contextOptions);
            mockContext.Setup(c => c.Bookings).Returns(mockSet.Object);

            var service = new BookingService(mockContext.Object);
            var custId = new List<int>();
            foreach (var book in bookingdata)
            {
                if (!custId.Contains(book.Customer.CustomerId))
                    custId.Add(book.Customer.CustomerId);
            }
            var id = custId[new Random().Next(custId.Count)];
            var bookings = service.GetBookingsByCustomerId(id).AsQueryable();
            Assert.Equal(bookingdata.Where(x => x.Customer.CustomerId == id), bookings);
        }

        [Fact]
        public void GetBookingsBetweenTimes()
        {
            var bookingdata = GenerateTestBookingData().AsQueryable();
            var mockSet = GetMockset(bookingdata);

            var contextOptions = new DbContextOptions<BookingContext>();
            var mockContext = new Mock<BookingContext>(contextOptions);
            mockContext.Setup(c => c.Bookings).Returns(mockSet.Object);

            var service = new BookingService(mockContext.Object);
            var startTime = DateTime.Now;
            var endtime = DateTime.Now.AddHours(3);
            var bookings = service.GetBookingsBetweenTimes(startTime, endtime).AsQueryable();

            Assert.Equal(bookingdata.Where(x => x.TimeOfVisit >= startTime && x.TimeOfVisit <= endtime), bookings);
        }

        [Fact]
        public void GetBookingsFromStartTime()
        {
            var bookingdata = GenerateTestBookingData().AsQueryable();
            var mockSet = GetMockset(bookingdata);

            var contextOptions = new DbContextOptions<BookingContext>();
            var mockContext = new Mock<BookingContext>(contextOptions);
            mockContext.Setup(c => c.Bookings).Returns(mockSet.Object);

            var service = new BookingService(mockContext.Object);
            var testTime = DateTime.Now;
            var bookings = service.GetBookingsFromStartTime(testTime).AsQueryable();
            
            Assert.Equal(bookingdata.Where(x => x.TimeOfVisit >= testTime), bookings);
        }

        [Fact]
        public void GetAllBookings()
        {
            var bookingdata = GenerateTestBookingData().AsQueryable();
            var mockSet = GetMockset(bookingdata);

            /*var bm = new List<BookingModel>();
            foreach (var mock in mockSet.Object)
            {
                bm.Add(new BookingModel{BookingId = mock.BookingId, Customer = mock.Customer, TimeOfVisit = mock.TimeOfVisit, EndTime = mock.EndTime});
            }
            var bmq = bm.AsQueryable();*/

            var contextOptions = new DbContextOptions<BookingContext>();
            var mockContext = new Mock<BookingContext>(contextOptions);
            mockContext.Setup(c => c.Bookings).Returns(mockSet.Object);

            var service = new BookingService(mockContext.Object);
            var bookings = service.GetAllBookings().AsQueryable();

            Assert.Equal(bookingdata,bookings);
        }

        public Mock<DbSet<BookingModel>> GetMockset(IQueryable<BookingModel> bookingdata)
        {
            var mockSet = new Mock<DbSet<BookingModel>>();
            mockSet.As<IQueryable<BookingModel>>().Setup(m => m.Provider).Returns(bookingdata.Provider);
            mockSet.As<IQueryable<BookingModel>>().Setup(m => m.Expression).Returns(bookingdata.Expression);
            mockSet.As<IQueryable<BookingModel>>().Setup(m => m.ElementType).Returns(bookingdata.ElementType);
            mockSet.As<IQueryable<BookingModel>>().Setup(m => m.GetEnumerator()).Returns(bookingdata.GetEnumerator());
            return mockSet;
        }

        public List<BookingModel> GenerateTestBookingData()
        {
            var cust = GenerateCustomerData();
            var bookings = new BookingModel[4]
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
                },
                new BookingModel()
                {
                    BookingId = 3,
                    Customer = cust[2],
                    TimeOfVisit = DateTime.Now.AddHours(-3),
                    EndTime = DateTime.Now.AddHours(-4)
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

