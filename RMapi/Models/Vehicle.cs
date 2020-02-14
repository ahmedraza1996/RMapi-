using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RMapi.Models
{
    public class Vehicle
    {
        public int DRIVERID { get; set; }
        public string REG_NO { get; set; }
        public string VEHICLE_TYPE { get; set; } 
        public int CAPACITY { get; set; }
        public string COLOR { get; set;  }
        public string MAKE { get; set;  }
        public int MODEL { get; set; }
        public string RUNNINGDOC { get; set; }

        //IMAGE
        public HttpPostedFileBase RUNNINGDOCIMAGE { get; set; }

    }
}