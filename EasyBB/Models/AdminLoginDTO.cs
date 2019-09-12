using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EasyBB.Models
{
    public class AdminLoginDTO
    {
        public string name { get; set; }

        public string pwd { get; set; }

        public string validatecode { get; set; }
    }
}