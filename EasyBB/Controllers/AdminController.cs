using EasyBB.Cores;
using EasyBB.Models;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EasyBB.Controllers
{
    public class AdminController : AdminBaseController
    {
       


        public ActionResult Index()//首页
        {
            //var loginAdmin = Session["currentAdmin"] as Admin;
            //if (loginAdmin == null)
            //{
            //    return Content("你没权访问");
            //}

            return View();
        }


        public ActionResult LoadMemu()//加载菜单方法
        {
            var admin = GetCurrentAdmin();
            var permissions = linqHelper.GetList<Permission>(m => m.adminid == admin.id).Select(m => m.actionid).ToList();
         
            var actionList = linqHelper.GetList<Action>(m=>permissions.Contains(m.id));//数据库的总表（根菜单和子菜单）
            var menuList = new List<MenuDTO>();//定义实体类
            var rootActionList = actionList.Where(m => m.pid == 0).ToList();//根菜单
            foreach (var item in rootActionList)
            {
                var menu = new MenuDTO() { id = item.id, text = item.name };//将数据库的根菜单赋值给实体数据
                var secondActionList = actionList.Where(m => m.pid == item.id).ToList();//查找数据库的子菜单

                foreach (var itemSecond in secondActionList)
                {
                    if (menu.children == null)
                    {
                        menu.children = new List<MenuDTO>();
                    }
                    menu.children.Add(new MenuDTO()
                    {
                        id = itemSecond.id,
                        text = itemSecond.name,
                        attributes = new
                        {
                            //将数据库的子菜单赋值给实体数据子集children
                            url = "/" + itemSecond.controller + "/" + itemSecond.action1
                        }
                    });
                }
                menuList.Add(menu);
            }
            return Json(menuList, JsonRequestBehavior.AllowGet);
        }
        public ActionResult MenuManage()//加载菜单
        {
            return View();
        }

        public ActionResult LoadMemuTreeGrid()//加载列表
        {
            var menuList = new List<MenuTreeGridDTO>();//定义实体类
            var actionList = linqHelper.GetList<Action>();//数据库的总表（根菜单和子菜单）
            var rootActionList = actionList.Where(m => m.pid == 0).ToList();//根菜单
            foreach (var item in rootActionList)
            {
                var menu = new MenuTreeGridDTO()
                {
                    pid = item.pid,
                    id = item.id,
                    name = item.name,
                    action = item.action1,
                    controller = item.controller,
                    addtime = item.addtime.ToShortDateString(),
                    enable = item.enable,
                    ismenu = item.ismenu
                };//将数据库的根菜单赋值给实体数据
                var secondActionList = actionList.Where(m => m.pid == item.id).ToList();//查找数据库的子菜单

                foreach (var itemSecond in secondActionList)
                {
                    if (menu.children == null)
                    {
                        menu.children = new List<MenuTreeGridDTO>();
                    }
                    var secondMenu = new MenuTreeGridDTO()
                    {
                        pid = itemSecond.pid,
                        id = itemSecond.id,
                        name = itemSecond.name,
                        action = itemSecond.action1,
                        controller = itemSecond.controller,
                        addtime = itemSecond.addtime.ToShortDateString(),
                        enable = itemSecond.enable,
                        ismenu = itemSecond.ismenu
                    };
                    var thirdActionList = actionList.Where(m => m.pid == itemSecond.id).ToList();
                    foreach (var itemThird in thirdActionList)
                    {

                        if (secondMenu.children == null)
                        {
                            secondMenu.children = new List<MenuTreeGridDTO>();
                        }
                        var thirdMenu = new MenuTreeGridDTO()
                        {
                            pid = itemThird.pid,
                            id = itemThird.id,
                            name = itemThird.name,
                            action = itemThird.action1,
                            controller = itemThird.controller,
                            addtime = itemThird.addtime.ToShortDateString(),
                            enable = itemThird.enable,
                            ismenu = itemThird.ismenu
                        };
                        secondMenu.children.Add(thirdMenu);
                    }

                    menu.children.Add(secondMenu);
                }
                menuList.Add(menu);
            }
            return Json(menuList, JsonRequestBehavior.AllowGet);
        }

        //public ActionResult UserManage()
        //{
        //    return View();
        //}


        
        public ActionResult Register()//注册
        {
            return View();
        }


        [HttpPost]
        public ActionResult Register(AdminDTO adminDto)//注册
        {
            Admin model = new Admin();
            model.name = adminDto.name;
            model.pwd = SecretyHelper.GetPassword(adminDto.pwd);
            model.addtime = DateTime.Now;
            model.avater = "default.png";
            model.lastloginip = IPHelper.GetClientIPv4Address();
            model.lastlogintime = DateTime.Now;
            model.registerip = model.lastloginip;
            model.registercity = IPHelper.GetIPCitys(model.registerip);
            model.status = 1;
            linqHelper.InsertEntity<Admin>(model);
            return View();
        }


        [NoCheckLogin]
        public ActionResult Login()//登录
        {
            return View();
        }     
        /// <summary>
        /// 登录的时候把信息给了Session,跳转到首页
        /// </summary>
        /// <param name="adminLoginDTO"></param>
        /// <returns></returns>
        [NoCheckLogin]//不需过滤
        [HttpPost]
        public ActionResult Login(AdminLoginDTO adminLoginDTO)//登录
        {
            var loginModel = linqHelper.GetEntity<Admin>(m => m.name.Equals(adminLoginDTO.name));//是否存在数据库
            if (loginModel!=null&&SecretyHelper.GetPassword(adminLoginDTO.pwd).Equals(loginModel.pwd)) 
            {
                Session["currentAdmin"] = loginModel;
                return Redirect("/Admin/Index");
            }
            else
            {

            }
            return View();
        }

        public ActionResult Permission()
        {

            return View();
        }


        [HttpPost]
        public ActionResult Handlemenu(string type,int id ,int? pid,bool? ismenu,string menucontroller,string menuaction,string menuname,bool? menuenable)
        {
            if(type!="delete"&&new string[] { menucontroller,menuaction,menuname}.Contains(string.Empty))
            {
                return Json(new {res=false,msg="提交的参数不正确"});
            }
            var model = new Action();
            model.action1 = menuaction;
            model.controller = menucontroller;
            model.addtime = DateTime.Now;
            model.name = menuname;
            model.enable = menuenable??false;
            switch (type)
            {
                case "addlevel"://新增同级菜单
                    model.pid = pid??0;
                    model.ismenu = ismenu??false;
                    linqHelper.InsertEntity(model);          
                    break;

                case "addbutton"://新增按钮(二级菜单)
                    model.pid = id;
                    model.ismenu = false;
                    linqHelper.InsertEntity(model);
                    break;
                case "addsubmenu"://新增下级菜单
                    model.pid = id;
                    model.ismenu = true;
                    linqHelper.InsertEntity(model);
                    break;

                case "edit":
                    model = linqHelper.GetEntity<Action>(m => m.id == id);
                    model.action1 = menuaction;
                    model.controller = menucontroller;
                    model.name = menuname;
                    model.enable = menuenable??false;
                    linqHelper.UpdateEntityNoAttach(model);
                    break;

                case "delete":
                    linqHelper.DeleteEntity<Action>(m => m.id == id);
                    break;
                
            }
            return Json(new { res = true });

        }

        public ActionResult AdminManage()//此方法产生了一个视图，然后他的数据从LoadAdmin获得
        {

            return View();
        }

        public ActionResult LoadAdmin()
        {
            var list = linqHelper.GetList<Admin>();
            return Json(new { total = 100, rows = list }, JsonRequestBehavior.AllowGet );
        }


        public ActionResult SetPermission(int id)
        {
            var model = linqHelper.GetEntity<Admin>(m=>m.id==id);
            ViewBag.Admin = model;
            return View();
        }
       
          

            [HttpPost]
        public ActionResult SavePermission(int[] ids,int adminId)
        {

            try
            {
                using (var db = new DataClassesDataContext())
                {
                    //if(db.Connection!=null)
                    //{
                    //    db.Connection.Open();
                    //}
                    //else
                    //{
                    //    return Json(false,JsonRequestBehavior.AllowGet);
                    //}
                    //DbTransaction tran = db.Connection.BeginTransaction();
                    //db.Transaction = tran;

                    var oldPermissions = db.Permission.Where(m => m.adminid == adminId);
                    db.Permission.DeleteAllOnSubmit(oldPermissions);
                    foreach (var item in ids)
                    {
                        db.Permission.InsertOnSubmit(new Permission() { actionid = item, addtime = DateTime.Now, adminid = adminId, enable = true });
                    }
                    db.SubmitChanges();
                    return Json(true, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception)
            {

                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult LoadPermissionTreeGrid(int id)//用户id
        {
            var permissions = linqHelper.GetList<Permission>(m => m.adminid == id).Select(m=>m.actionid).ToList();//刷选出当前用户，获得Actionid
            var menuList = new List<MenuTreeGridDTO>();//定义实体类
            var actionList = linqHelper.GetList<Action>();//数据库的总表（根菜单和子菜单）
            var rootActionList = actionList.Where(m => m.pid == 0).ToList();//根菜单
            foreach (var item in rootActionList)
            {
                var menu = new MenuTreeGridDTO()
                {
                    pid = item.pid,
                    id = item.id,
                    name = item.name,
                    action = item.action1,
                    controller = item.controller,
                    addtime = item.addtime.ToShortDateString(),
                    enable = item.enable,
                    ismenu = item.ismenu,
                    @checked=permissions.Contains(item.id)//用户表的id就是permission的actionid
                };//将数据库的根菜单赋值给实体数据
                var secondActionList = actionList.Where(m => m.pid == item.id).ToList();//查找数据库的子菜单

                foreach (var itemSecond in secondActionList)
                {
                    if (menu.children == null)
                    {
                        menu.children = new List<MenuTreeGridDTO>();
                    }
                    var secondMenu = new MenuTreeGridDTO()
                    {
                        pid = itemSecond.pid,
                        id = itemSecond.id,
                        name = itemSecond.name,
                        action = itemSecond.action1,
                        controller = itemSecond.controller,
                        addtime = itemSecond.addtime.ToShortDateString(),
                        enable = itemSecond.enable,
                        ismenu = itemSecond.ismenu,
                        @checked = permissions.Contains(itemSecond.id)
                    };
                    var thirdActionList = actionList.Where(m => m.pid == itemSecond.id).ToList();
                    foreach (var itemThird in thirdActionList)
                    {

                        if (secondMenu.children == null)
                        {
                            secondMenu.children = new List<MenuTreeGridDTO>();
                        }
                        var thirdMenu = new MenuTreeGridDTO()
                        {
                            pid = itemThird.pid,
                            id = itemThird.id,
                            name = itemThird.name,
                            action = itemThird.action1,
                            controller = itemThird.controller,
                            addtime = itemThird.addtime.ToShortDateString(),
                            enable = itemThird.enable,
                            ismenu = itemThird.ismenu,
                            @checked = permissions.Contains(itemThird.id)
                        };
                        secondMenu.children.Add(thirdMenu);
                    }

                    menu.children.Add(secondMenu);
                }
                menuList.Add(menu);
            }
            return Json(menuList, JsonRequestBehavior.AllowGet);
        }

        public ActionResult UserList()
        {
            return View();
        }

        [HttpGet]
        public ActionResult GetUserList(int page,int rows)
        {
            var list = linqHelper.GetListByPage<User>(page, rows);
            var total = linqHelper.Count<User>();
            return Json(new { total = total, rows = list }, JsonRequestBehavior.AllowGet);
            
        }

        public ActionResult SetStatus(int id, int status)
        {
            int res = 0;
            try
            {
                var user = linqHelper.GetEntity<User>(m => m.id == id);
                if (user != null)
                {
                    user.status = (status == 1 ? 0 : 1);
                    linqHelper.UpdateEntityNoAttach(user);
                    res = 1;
                }
            }
            catch (Exception)
            {
                //记录日志
            }
            return Json(new { res = res }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult BorarList()
        {
            return View();
        }

    }
}