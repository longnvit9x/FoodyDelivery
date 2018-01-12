using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Foody.Models;
namespace Foody.Areas.Administrator.Models
{
    public class AdminHomeController : Controller
    {
        private FoodDeliveryEntities db;
        public AdminHomeController()
        {
            db = new FoodDeliveryEntities();
        }
        // GET: Administrator/AdminHome
        public ActionResult Index()
        {
            if (Session["Admin"] == null)
            {
                return RedirectToRoute("Default", new { controller = "Home", action = "Index", id = UrlParameter.Optional });
            }
            return View();
        }

        public ActionResult profile()
        {
            Customer cus= (Customer)Session["Admin"];
            ViewBag.FullName = "";
            FileData fileData = db.FileDatas.Where(p => p.FileID == cus.FileID).SingleOrDefault();
            if (fileData != null && fileData.URL != null)
            {
                ViewBag.Avatar = fileData.URL;
            }
            else
            {
                ViewBag.Avatar = cus.Avatar;
            }
            return PartialView(cus);
        }

        public ActionResult Logout()
        {
            if (Session["Admin"] != null)
            {
                Session.Clear();
            }
            return RedirectToRoute("Default", new { controller = "Home", action = "Index", id = UrlParameter.Optional });
        }
    }
}