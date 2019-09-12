using EasyBB.Cores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EasyBB.Controllers
{
    public class AdminBaseController : BaseController
    {
     // protected LinqHelper<DataClassesDataContext> linqHelper = new LinqHelper<DataClassesDataContext>();
        protected Admin GetCurrentAdmin()
        {
            return Session["currentAdmin"] as Admin;
        }
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            //判断有没有打上标注
            bool isNoCheckLogin = filterContext.ActionDescriptor.IsDefined(typeof(NoCheckLoginAttribute), false);
            if(!isNoCheckLogin)//如果需要验证
            {
                var loginAdmin = Session["currentAdmin"] as Admin;
                
                if (loginAdmin == null)
                {
                    //跳转到登录页面
                    filterContext.Result = Redirect("/admin/login");
                }
                else
                {
                    var controllerName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName.ToLower();
                    var actionName = filterContext.ActionDescriptor.ActionName.ToLower();
                    //特殊处理后台首页和加载菜单
                    if(!(controllerName.Equals("admin")&& (actionName.Equals("index")||actionName.Equals("loadmemu"))))
                    {
                        var admin = GetCurrentAdmin();
                       // var permissions = linqHelper.GetList<Permission>(m => m.adminid == admin.id).Select(m => m.actionid).ToList();
                        using (var db = new DataClassesDataContext())
                        {
                            var q = from p in db.Permission
                                    join a in db.Action
                                    on p.actionid equals a.id
                                    where p.adminid == admin.id && a.action1.ToLower().Equals(actionName) &&
                                    a.controller.ToLower().Equals(controllerName)
                                    select p;
                            var res = q.Any();
                            if (!res)
                            {
                                filterContext.Result = Redirect("/admin/login");
                            }
                        }
                    }
                }
            }

            

        }
    }
}