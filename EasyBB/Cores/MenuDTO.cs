using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EasyBB.Cores
{
    public class MenuDTO
    {
        public int id { get; set; }
        public string text { get; set; }
        public List<MenuDTO> children { get; set; }

        public object attributes { get; set; }
    }

    public class MenuTreeGridDTO
    {
        public int id { get; set; }
        public int pid { get; set; }
        public string name { get; set; }
        public string controller { get; set; }

        public string action { get; set; }
        public bool ismenu { get; set; }
        public bool enable { get; set; }
        public string addtime { get; set; }

        public bool @checked { get; set;}
        public List<MenuTreeGridDTO> children { get; set; }

      
    }
}