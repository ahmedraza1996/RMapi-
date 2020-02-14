using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RMapi.Models
{
    public class Passenger_Transaction_History
    {
        public int USERID { get; set; }
        public char TRANSACTIONTYPE { get; set; }
        public int AMOUNT { get; set; }
        public DateTime? DATE { get; set; }
    }
}