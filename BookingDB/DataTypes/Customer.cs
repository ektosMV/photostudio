using System;
using System.Collections.Generic;
using System.Text;

namespace BookingDB.DataTypes
{
    public class Customer
    {
        public int CustomerId { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
    }
}
