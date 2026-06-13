using RajasthanTourCabN.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RajasthanTourCabN.Controllers
{
    public class AccountController : Controller
    {
        PageService service = new PageService();

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(string username, string password)
        {
            if (service.CheckLogin(username, password))
            {
                var role = service.GetUserRole(username, password);
                if (role != null)
                {
                    Session["Admin"] = username;
                    Session["Role"] = role;

                    return RedirectToAction("Dashboard", "Admin");
                }
            }

            ViewBag.Error = "Invalid Login";
            return View();
        }

        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Index");
        }

        public ActionResult AdminMenu()
        {
            var menu = service.GetAdminMenu(Session["Role"].ToString());
            return PartialView("_AdminMenu", menu);
        }
    }
}