using EasyBB.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EasyBB.Controllers
{
    public class ThemeController : HomeBaseController
    {
        // GET: Theme
        public ActionResult Add(int id)
        {
            ViewBag.ID = id;
            return View();
        }

        [HttpPost]
        public ActionResult Add(ThemeViewModel viewModel, int id)
        {
            if (ModelState.IsValid)
            {
                var user = GetCurrentUser();
                if (user == null)
                {
                    return RedirectToAction("Login", "User");
                }
                var model = new Thems();
                model.addtime = DateTime.Now;
                model.borderid = id;
                model.lastlevel = 0;
                model.status = 1;
                model.title = viewModel.Title;
                model.ups = 0;
                model.downs = 0;
                model.content = viewModel.Content;
                model.userid = user.id;
                try
                {
                    linqHelper.InsertEntity<Thems>(model);
                    return Redirect("/home/board/" + id);
                }
                catch (Exception ex)
                {

                    return ShowErrors(ex.Message);
                }
            }
            return View(viewModel);
        }

        //public ActionResult Detail()
        //{
        //    return View();
        //}

        [HttpPost]
        public ActionResult SavePosts(string content, int? tid)
        {
            int res = 0;
            try
            {
                var user = GetCurrentUser();
                if (user != null)
                {
                    var theme = linqHelper.GetEntity<Thems>(m => m.id == tid);
                    var posts = new Posts();
                    posts.addtime = DateTime.Now;
                    posts.borderid = theme.borderid;
                    posts.collectcount = 0;
                    posts.content = content;
                    posts.downs = 0;
                    posts.ups = 0;
                    posts.userid = user.id;
                    posts.level = theme.lastlevel + 1; ;
                    posts.status = 1;
                    posts.themeid = theme.id;
                    linqHelper.InsertEntity(posts);
                    res = 1;
                    theme.lastlevel++;
                    linqHelper.UpdateEntityNoAttach(theme);

                }
            }
            catch (Exception)
            {

            }
            return Json(new { res = res }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Detail(int id, int p = 1)
        {
            p = p < 0 ? 1 : p;
            var list = linqHelper.GetListByPage<Posts>(p, 5);
            ViewBag.Thems = linqHelper.GetEntity<Thems>(m => m.id == id);
            ViewBag.Total = linqHelper.Count<Posts>();
            return View(list);
        }
    }
}