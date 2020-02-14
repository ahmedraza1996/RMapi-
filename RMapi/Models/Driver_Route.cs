using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RMapi.Models
{
    public class Driver_Route
    {
        public int ROUTEID { get; set; }
        public int DRIVERID { get; set; }
        public DateTime? DATE { get; set; }
        public string FROM { get; set; }
        public string TO { get; set; }
        public string TIMESLAB { get; set; }
        public int AVAILABLESEATS { get; set; }

    }
}