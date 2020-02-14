using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RMapi.Models
{
    public class Passenger_Request
    {
        public int  ROUTEID { get; set; }
        public int USERID { get; set; }
        public DateTime? DATE { get; set; }
        public string FROM { get; set; }
        public string TO { get; set;  }
        public string TIMESLAB { get; set;  }
        public char STATUS { get; set;  }

    }
}