using EasyBB.Cores;
using EasyBB.Models;
using EasyBB.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace EasyBB.Controllers
{
    public class UserController : HomeBaseController
    {
        // GET: User
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register1(string name, string pwd, string cpwd,string email)
        {
            Regex regx = new Regex("^[\u4e00-\u9fa5_a-zA-z0-9]+$");
            Regex regxEmail = new Regex("^[a-zA-Z0-9_.-]+@[a-zA-Z0-9-]+(\\.[a-zA-Z0-9-]+)*\\.[a-zA-Z0-9]{2,6}$");
            bool isValid = true;
            string errors = string.Empty;
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(pwd) || string.IsNullOrEmpty(cpwd))
            {
                errors += "数据不能为空";
                // ModelState.AddModelError("error", "数据不能为空");
                isValid = false;
            }
            else if (pwd != cpwd)
            {
                errors += "两次密码输入不一致";
                // ModelState.AddModelError("error", "账号或密码格式不对");
                isValid = false;
            }
            else if ((pwd.Length < 6 || pwd.Length > 15) || (name.Length < 3 || name.Length > 20))
            {
                errors += "账号或密码格式不对";
                // ModelState.AddModelError("error", "账号或密码格式不对");
                isValid = false;
            }

            else if (!regx.IsMatch(name))
            {
                errors += "账号格式不对";
                //  ModelState.AddModelError("error", "账号格式不对");
                isValid = false;
            }
            else if (!regxEmail.IsMatch(email))
            {
                errors += "邮箱格式不对";
                // ModelState.AddModelError("error", "账号格式不对");
                isValid = false;
            }
            if (!isValid)
            {
                return ShowErrors(errors);
               // return Content(errors);

            }
            User model = new User();
            model.name = name;
            model.pwd = SecretyHelper.GetPassword(pwd);
            model.addtime = DateTime.Now;
            model.avatar = "default.png";
            model.lastloginip = IPHelper.GetClientIPv4Address();
            model.lastlogintime = DateTime.Now;
            model.registerip = model.lastloginip;
            model.status = 1;
            model.niname = model.name;
            model.postcount = "1";
            model.email = email;
            try
            {
                linqHelper.InsertEntity<User>(model);
                return RedirectToAction("login");
            }
            catch (Exception ex)
            {

                //ModelState.AddModelError("error", ex.Message);
                //return View();
                errors += ex.Message;
                return Content(errors);
            }
            

            }

        [HttpPost]
        public ActionResult Register(UserRegistViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                User model = new User();
                model.name = viewModel.Name;
                model.pwd = SecretyHelper.GetPassword(viewModel.Pwd);
                model.addtime = DateTime.Now;
                model.avatar = "default.png";
                model.lastloginip = IPHelper.GetClientIPv4Address();
                model.lastlogintime = DateTime.Now;
                model.registerip = model.lastloginip;
                model.status = 1;
                model.niname = model.name;
                model.email = viewModel.Email;
                model.postcount = "1";
                try
                {
                    linqHelper.InsertEntity<User>(model);
                    return RedirectToAction("Login");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("error", ex.Message);
                }
            }
            return View(viewModel);
        }

        public ActionResult Login()
        {
            return View();
        }


    
  
       
        [HttpPost]
        public ActionResult Login(UserLoginDTO userLoginDTO)//登录
        {
            if(ModelState.IsValid)
            {
                var loginModel = linqHelper.GetEntity<User>(m => m.name.Equals(userLoginDTO.Name));//是否存在数据库
                if (loginModel != null && SecretyHelper.GetPassword(userLoginDTO.Pwd).Equals(loginModel.pwd))
                {
                    Session["currentUser"] = loginModel;
                    return Redirect("/");
                }
                else
                {

                }
            }
            
            return View(userLoginDTO);
        }

    }
}
