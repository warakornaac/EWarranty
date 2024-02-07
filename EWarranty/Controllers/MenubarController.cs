using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EWarranty.Controllers
{
    public class MenubarController : Controller
    {
        //
        // GET: /Menubar/

        public ActionResult Index()
        {
            string User = Session["UserID"].ToString();
            string UserType = Session["UserType"].ToString();
            ViewBag.UserId = User;
            ViewBag.UserType = UserType;

            return View();
        }

    }
}
