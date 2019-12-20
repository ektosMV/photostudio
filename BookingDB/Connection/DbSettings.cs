using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace BookingDB.Connection
{
    public enum DbTypes
    {
        MySQL
    }
    [DataContract]
    public class DbSettings
    {
        [DataMember]
        public Dictionary<string,string> ConnectionStrings { get; set; }
        [DataMember]
        public string PreferedProvider { get; set; }
    }
}
