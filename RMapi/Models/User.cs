using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RMapi.Models
{
    public class User
    {
        public int USERID { get; set; }
        public string FIRSTNAME { get; set; }
        public string LASTNAME { get; set; }
        public string NU_ID { get; set; }
        public string PASSWORD { get; set; }
        public string CONFIRM_PASSWORD { get; set;  }
        public string USERNAME { get; set;  }
        public string GENDER { get; set;  }
        public string PHONENUMBER { get; set;  }
    }
}