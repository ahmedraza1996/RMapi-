using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RMapi.Models
{
    public class Driver_Transaction_History
    {
        public int DRIVERID { get; set; }
        public char TRANSACTIONTYPE { get; set; }
        public int AMOUNT { get; set; }
        public DateTime? DATE { get; set; }
    }
}