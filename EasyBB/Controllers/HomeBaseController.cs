using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EasyBB.Controllers
{
    public class HomeBaseController : BaseController
    {
        // GET: HomeBase
        protected User GetCurrentUser()
        {
            return Session["currentUser"] as User;
        }
    }
}