using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Foody.Models;
using PagedList;
using System.IO;

namespace Foody.Controllers
{
    public class StoreController : Controller
    {
        int pageSize = 9;
        private FoodDeliveryEntities db;
        public StoreController()
        {
            db = new FoodDeliveryEntities();
        }
        // GET: Store

        public ActionResult Index()
        {
            return View();
        }
        // GET: Store
        public ActionResult XemChiTiet(Guid StoreID)
        {

            Store store = db.Stores.SingleOrDefault(n => n.StoreID == StoreID);
            if (store == null)
            {
                //Trả về trang báo lỗi 
                Response.StatusCode = 404;
                return null;
            }
            else
            {
                if (Session["UserName"] != null)
                {
                    string user = Session["UserName"].ToString();
                    Customer cus = db.Customers.SingleOrDefault(n => n.UserName == user);
                    if (store.CustomerID.Equals(cus.CustomerID))
                    {
                        ViewBag.StoreAdmin = 1;
                        return View(store);
                    }
                }
            }
            ViewBag.StoreID = store.StoreID;
            return View(store);
        }

        public ActionResult ThongTin(Guid StoreID)
        {
            Store store = db.Stores.SingleOrDefault(n => n.StoreID == StoreID);
            if (store == null)
            {
                //Trả về trang báo lỗi 
                Response.StatusCode = 404;
                return null;
            }
            ViewBag.StoreID = StoreID;
            return View(store);
        }
        [HttpPost]
        public ActionResult ThongTin(Store store, HttpPostedFileBase image)
        {
            Store st = db.Stores.Where(n => n.StoreID == store.StoreID).SingleOrDefault();
            if (st == null)
            {
                Response.StatusCode = 404;
                return null;
            }
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
                fileData.URL = "/Data/Originals/" + filename;
                db.FileDatas.Add(fileData);
                db.SaveChanges();
                st.FileID = fileData.FileID;
            }
            st.StoreName = store.StoreName;
            st.Address = store.Address;
            st.OpenDoor = store.OpenDoor;
            st.CloserDoor = store.CloserDoor;
            st.ServiceCharge = store.ServiceCharge;
            st.ShippingFee = store.ShippingFee;
            st.Manner = store.Manner;
            st.Website = store.Website;
            st.StoreType = store.StoreType;
            st.StoreBanner = store.StoreBanner;
            st.StartDate = store.StartDate;
            st.EndDate = store.EndDate;
            st.Rating = 0;
            st.Evaluation = 0;
            st.ConditionShip = store.ConditionShip;
            if (ModelState.IsValid)
            {
                //Thực hiện cập nhận trong model
                db.Entry(st).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                ViewBag.Message = store.StoreName + " Cập nhật thành công";
            }
            ViewBag.StoreID = store.StoreID;
            return View(store);
        }

        public ActionResult MenuStoreAdmin(string StoreID)
        {
            ViewBag.StoreID = StoreID;
            return PartialView();
        }

        public ActionResult Menuleft()
        {
            return PartialView();
        }

        public ActionResult StoreDetail(Guid StoreID)
        {
            var foods = db.Foods.Where(n => n.StoreID == StoreID).ToList();
            return PartialView(foods);
        }

        public ActionResult ThemStore()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ThemStore(Store store, HttpPostedFileBase image)
        {
            store.StoreID = Guid.NewGuid();
            store.CustomerID = Guid.Parse(Session["CustomerID"].ToString());
            store.Rating = 0;
            store.Evaluation = 0;
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
                fileData.URL = "/Data/Originals/" + filename;
                db.FileDatas.Add(fileData);
                db.SaveChanges();
                store.FileID = fileData.FileID;
            }

            if (ModelState.IsValid)
            {
                //Thực hiện cập nhận trong model
                db.Entry(store).State = System.Data.Entity.EntityState.Added;
                db.SaveChanges();
                ViewBag.Message = store.StoreName + " Thêm thành công";
            }
            return View(store);
        }

        //Store Sale
        public ActionResult QuanLySale(string StoreID, int page, string method)
        {
            var stID = Guid.Parse(StoreID);
            if (method == null)
            {
                Session["Message"] = null;
            }
            var listStoreSale = db.StoreSales.Where(n => n.StoreID == stID).OrderBy(x => x.StopSale).ToPagedList(page, pageSize);
            if (listStoreSale == null )
            {
                //Trả về trang báo lỗi 
                Response.StatusCode = 404;
                return null;
            }
            ViewBag.StoreID = StoreID;
            return View(listStoreSale);
        }


        public ActionResult ThemSale(string StoreID)
        {
            ViewBag.StoreID = StoreID;
            return View();
        }

        [HttpPost]
        public ActionResult ThemSale(StoreSale storeSale, string StoreID)
        {
            storeSale.StoreID = Guid.Parse(StoreID);
            if (ModelState.IsValid)
            {
                //Thực hiện cập nhận trong model
                db.Entry(storeSale).State = System.Data.Entity.EntityState.Added;
                db.SaveChanges();
                Session["Message"] = "Thêm thành công";
                return RedirectToAction("QuanLySale", new { StoreID = StoreID, page = 1, method = "ThemMoi" });
            }
            ViewBag.StoreID = StoreID;
            return View(storeSale);
        }

        public ActionResult SuaSale(int StoreSaleID, string StoreID)
        {
            var stID = Guid.Parse(StoreID);
            var storeSale = db.StoreSales.Where(n => n.StoreSaleID == StoreSaleID).SingleOrDefault();
            if (storeSale == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            DateTime dateNow = DateTime.Today;
            if (storeSale.StartSale <= dateNow && storeSale.StopSale >= dateNow)
            {
                Session["Message"] = "Đang trong thời gian sale không thể sửa";
                ViewBag.StoreID = StoreID;
                return RedirectToAction("QuanLySale", new { StoreID = StoreID, page = 1, method = "Sua" });
            }
            ViewBag.StoreID = StoreID;
            return View(storeSale);
        }

        [HttpPost]
        public ActionResult SuaSale(StoreSale storeSale, string StoreID)
        {
            var ss = db.StoreSales.Where(n => n.StoreSaleID == storeSale.StoreSaleID).SingleOrDefault();
            if (ss == null)
            {
                //Trả về trang báo lỗi 
                Response.StatusCode = 404;
                return null;
            }

            ss.Sale = storeSale.Sale;
            ss.StartSale = storeSale.StartSale;
            ss.StopSale = storeSale.StopSale;
            if (ModelState.IsValid)
            {
                //Thực hiện cập nhận trong model
                db.Entry(ss).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                Session["Message"] = "Sửa thành công";
                return RedirectToAction("QuanLySale", new { StoreID = StoreID, page = 1, method = "Sua" });
            }
            ViewBag.StoreID = StoreID;
            return View(storeSale);
        }

        public ActionResult XoaSale(int StoreSaleID, string StoreID)
        {
            var stID = Guid.Parse(StoreID);
            var storeSale = db.StoreSales.Where(n => n.StoreSaleID == StoreSaleID).SingleOrDefault();
            if (storeSale == null)
            {
                //Trả về trang báo lỗi 
                Response.StatusCode = 404;
                return null;
            }
            DateTime dateNow = DateTime.Today;
            if(storeSale.StartSale<= dateNow && storeSale.StopSale>= dateNow)
            {
                Session["Message"] = "Đang trong thời gian sale không thể xóa";
                ViewBag.StoreID = StoreID;
                return RedirectToAction("QuanLySale", new { StoreID = StoreID, page = 1, method = "Xoa" });
            }

            db.Entry(storeSale).State = System.Data.Entity.EntityState.Deleted;
            db.SaveChanges();
            Session["Message"] = "Xóa thành công";
            ViewBag.StoreID = StoreID;
            return RedirectToAction("QuanLySale", new { StoreID = StoreID, page = 1, method = "Xoa" });
        }

        //Store Sale
        public ActionResult QuanLyStore(int page, string method)
        {
            if (Session["UserName"] != null)
            {
                Customer cus =(Customer) Session["TaiKhoan"];
                if (method == null)
                {
                    Session["Message"] = null;
                }
                var listStore = db.Stores.Where(n => n.CustomerID == cus.CustomerID).OrderBy(x => x.StoreName).ToPagedList(page, pageSize);
                if (listStore == null)
                {
                    //Trả về trang báo lỗi 
                    Response.StatusCode = 404;
                    return null;
                }
                return View(listStore);
            }
            return null;
        }
    }
}