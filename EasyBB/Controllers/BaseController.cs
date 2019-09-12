using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EasyBB.Controllers
{
    public class BaseController : Controller
    {
        protected Cores.LinqHelper<DataClassesDataContext> linqHelper = new Cores.LinqHelper<DataClassesDataContext>();
        public ActionResult ShowErrors(string message)
        {
            return View("~/Views/Shared/ShowErrors.cshtml", null, message);
        }
    }
}