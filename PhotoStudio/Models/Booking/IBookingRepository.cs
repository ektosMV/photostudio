using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhotoStudio.Models.Booking
{
    interface IBookingRepository<T> : IDisposable where T : class
    {
        IEnumerable<T> GetBookingList();
        T GetBooking(int id);
        void Create(T item);
        void Update(T item);
        void Delete(int id);
        void Save();
    }
}
