using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EasyBB.Cores
{
    [AttributeUsage(AttributeTargets.Class|AttributeTargets.Method)]
    public class NoCheckLoginAttribute: Attribute
    {
    }
}