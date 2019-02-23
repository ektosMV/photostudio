using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;
using PhotoStudio;
using PhotoStudio.Models;
using PhotoStudio.Models.Booking;
using PhotoStudio.Modules;

namespace xUnitTestApp.Tests
{
    public class GoogleCalendarHandlerTest
    {
        private CalendarHandler _calendarHandler;
        public GoogleCalendarHandlerTest()
        {
            _calendarHandler = new CalendarHandler();
        }
        [Fact]
        public void AddEventRequestTest()
        {
            var starttime = new DateTime(DateTime.Now.Ticks).AddDays(1);
            var endTime = new DateTime(DateTime.Now.Ticks).AddDays(1).AddHours(1);
            _calendarHandler.AddEventRequest("Test", starttime, endTime, "Asia/Yekaterinburg");
            
        }

        [Fact]
        public void GetEventsRequewst()
        {
            var starttime = DateTime.Now.AddDays(1);
            var endTime = DateTime.Now.AddDays(1).AddHours(1);
            _calendarHandler.DeleteAllEvents();
            _calendarHandler.AddEventRequest("Test", starttime, endTime, "Asia/Yekaterinburg");
            var res = _calendarHandler.GetEventRequest(starttime.AddDays(-1), endTime.AddDays(1));
            Assert.Equal(1, res.Count());
        }

        [Fact]
        public void DeleteRequestTest()
        {
            _calendarHandler.DeleteAllEvents();
            Assert.Equal(0, GetCountOfEvents());
        }
        
        public int GetCountOfEvents()
        {
            return _calendarHandler.GetEventRequest(DateTime.MinValue).Count;
        }

        [Fact]
        public void dd()
        {
            var l = new LocalBaseTests();

        }

        [Fact]
        public void ddd()
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

            var service = new CalendarSynchronise(mockContext.Object);
            //service.AddNewBookingsFromGoogleCaledar();
            

            service.GetEventsByDays(DateTime.Now.Date, 4);
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
