using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Foody.Models;
using System.IO;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace Foody.Controllers
{
    public class AccountController : Controller
    {
        private FoodDeliveryEntities db;
        public AccountController()
        {
            db = new FoodDeliveryEntities();
        }
        // GET: Account
        public ActionResult Index()
        {
            return View(db.Customers.ToList());
        }

        public ActionResult Register()
        {
            return View();
        }

        public ActionResult ThongBaoPartial()
        {
            List<Post> lstPost = new List<Post>();
            if (Session["TaiKhoan"] != null)
            {
                Customer cus = (Customer)Session["TaiKhoan"];
                lstPost = db.Posts.Where(n => n.CustomerID == cus.CustomerID)
                    .OrderByDescending(n => n.PostTime).ToList();
                ViewBag.PostNew = lstPost;
                return PartialView();
            }
           
            return PartialView();
        }

        [HttpPost]
        public ActionResult Register(Customer cus, HttpPostedFileBase image)
        {
            cus.CustomerID = Guid.NewGuid();
            cus.UserType = "web";

            if (image != null)
            {
                FileData fileData = new FileData();
                //attach the uploaded image to the object before saving to Database
                fileData.FileType = image.ContentType;
                image.InputStream.Read(new byte[image.ContentLength], 0, image.ContentLength);

                //Save image to file
                var filename = image.FileName;
                var filePathOriginal = Server.MapPath("/Data/Originals");
                var filePathThumbnail = Server.MapPath("/Data/Thumbnails");
                string savedFileName = Path.Combine(filePathOriginal, filename);
                image.SaveAs(savedFileName);
                fileData.FileName = filename;
                fileData.URL = "/Data/Originals/"+filename;
                db.FileDatas.Add(fileData);
                db.SaveChanges();

                cus.FileID = fileData.FileID;
            }
            db.Customers.Add(cus);
            db.SaveChanges();
            ModelState.Clear();
            ViewBag.Message = cus.FullName + "đăng kí thành công";
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(FormCollection f)
        {
            string userName = f["txtUserName"].ToString();
            string password = f["txtPassword"].ToString();
            if(userName=="admin" && password == "admin")
            {
                Session["Admin"] = new Customer();
                return RedirectToRoute("Administrator_default", new { controller = "AdminHome", action = "Index", id = UrlParameter.Optional });
            }
            var Cus = db.Customers.Where(u => u.UserName == userName && u.Password == password).SingleOrDefault();
            if (Cus != null)
            {
                Session["FullName"] = Cus.FullName.ToString();
                Session["UserName"] = Cus.UserName.ToString();
                Session["CustomerID"] = Cus.CustomerID.ToString();
                Session["TaiKhoan"] = Cus;
                return RedirectToAction("LoggedIn");
            }
            else
            {
                ModelState.AddModelError("", "Tài khoản và mật khẩu không đúng...!");
            }
            return View();
        }
        public ActionResult LoggedIn()
        {
            if (Session["FullName"] != null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

        public ActionResult AccountDetail()
        {
            if (Session["FullName"] != null)
            {
                Customer cus = (Customer)Session["TaiKhoan"];
                return View(cus);
            }
            return null;
        }

        [HttpPost]
        public ActionResult AccountDetail(Customer customer)
        {
            if (Session["FullName"] != null)
            {
                Customer cus = db.Customers.Where(n => n.CustomerID == customer.CustomerID).SingleOrDefault();
                if (cus == null)
                {
                    return null;
                }
                cus.FullName = customer.FullName;
                cus.Address = customer.Address;
                cus.Mobile = customer.Mobile;
                cus.Password = customer.Password;
                cus.Email = customer.Email;
                cus.Avatar = customer.Avatar;
                cus.FullName = customer.FullName;
                if (ModelState.IsValid)
                {
                    db.Entry(cus).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    ViewBag.Message = "Cập nhật thành công";
                    Session["TaiKhoan"] = cus;
                    return View(cus);
                }
            }
            return View(customer);
        }

        public ActionResult HistoryInvoice()
        {
            if (Session["FullName"] != null)
            {
                Customer cus = (Customer)Session["TaiKhoan"];
                List<Invoice> lstInvoice = db.Invoices.Where(n => n.CustomerID == cus.CustomerID)
                    .OrderByDescending(n=>n.OrderDate).ToList();
                return View(lstInvoice);
            }
            return RedirectToAction("Index");
        }

        public ActionResult Logout()
        {
            if (Session["FullName"] != null)
            {
                Session.Clear();
                ViewBag.FullName = null;
            }
            return RedirectToAction("Index", "Home");
        }

    }
}