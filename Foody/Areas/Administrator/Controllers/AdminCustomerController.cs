using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Foody.Models;

namespace Foody.Areas.Administrator.Controllers
{
    public class AdminCustomerController : Controller
    {
        private FoodDeliveryEntities db;
        public AdminCustomerController()
        {
            db = new FoodDeliveryEntities();
        }
        public ActionResult DanhSachKhachHang()
        {
            List<Customer> lst = new List<Customer>();
            lst = db.Customers.OrderBy(n => n.FullName).ToList();
            return View(lst);
        }

    }
}