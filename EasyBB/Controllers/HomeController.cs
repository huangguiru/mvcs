using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EasyBB.Controllers
{
    public class HomeController : HomeBaseController
    {
        public ActionResult Index()
        {
           var list= linqHelper.GetList<Board>(m => m.status == 1);
            return View(list);
        }
        public ActionResult Board(int id,int p=1)
        {
            var list = linqHelper.GetListByPage<Thems>(p,5);
            ViewBag.Board = linqHelper.GetEntity<Board>(m => m.id == id);
            ViewBag.Total = linqHelper.Count<Thems>();
            return View(list);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}