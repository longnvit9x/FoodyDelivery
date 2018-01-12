using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Foody.Models;
using Foody.Models.Entity;
using Foody.Models.Dto;
using PagedList;
using System.IO;
using System.Globalization;

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
                    if (store.Status == 0)
                    {
                        return RedirectToAction("Index", "Home");
                    }
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
            if (Session["UserName"] != null)
            {
                Customer cus = (Customer)Session["TaiKhoan"];
                if (!store.CustomerID.Equals(cus.CustomerID))
                {
                    return RedirectToAction("XemChiTiet", new { StoreID = StoreID });
                }
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
            var stID = Guid.Parse(StoreID);
            Store store = db.Stores.SingleOrDefault(n => n.StoreID == stID);
            ViewBag.StoreID = StoreID;
            ViewBag.StoreName = store.StoreName;
            return PartialView();
        }

        public ActionResult Menuleft()
        {
            return PartialView();
        }

        public ActionResult ThongKe(string StoreID)
        {
            var stID = Guid.Parse(StoreID);
            Store store = db.Stores.SingleOrDefault(n => n.StoreID == stID);
            ViewBag.StoreID = StoreID;
            ViewBag.StoreName = store.StoreName;
            return View(store);
        }

        [HttpGet]
        public JsonResult getThongKe(DataDateDto data)
        {
            var stID = Guid.Parse(data.StoreID);
            DateTime date = DateTime.ParseExact(data.Month + "/" + data.Year, "M/yyyy", CultureInfo.InvariantCulture);
            var store = db.Stores.SingleOrDefault(n => n.StoreID == stID);
            if (store == null)
            {
                return null;
            }
            var invoices = db.Invoices.Where(n => n.StoreID == stID && n.OrderDate.Value.Month == date.Month && n.OrderDate.Value.Year == date.Year)
                .GroupBy(n => n.OrderDate.Value.Day)
                 .Select(n => new { ngay = n.Key, sodondat = n.Count() })
                .ToList();
            List<InvoiceDto> lstIV = new List<InvoiceDto>();
            foreach (var detail in invoices)
            {
                InvoiceDto ivDto = new InvoiceDto
                {
                    OrderDate = detail.ngay.ToString(),
                    NumberInvoice = detail.sodondat
                };
                lstIV.Add(ivDto);
            }
            List<PostDto> lstpost = new List<PostDto>();
            var posts = db.Posts.Where(n => n.StoreID == stID && n.PostTime.Value.Month == date.Month && n.PostTime.Value.Year == date.Year)
                 .GroupBy(n => n.PostTime.Value.Day)
                 .Select(n => new { ngay = n.Key, sobaiviet = n.Count() })
                .ToList();

            foreach (var pt in posts)
            {
                PostDto cmt = new PostDto
                {
                    PostTime = pt.ngay.ToString(),
                    NumberPost = pt.sobaiviet
                };
                lstpost.Add(cmt);
            }

            var invoiceDoanhThu = db.Invoices.Where(n => n.StoreID == stID && n.OrderDate.Value.Month == date.Month && n.OrderDate.Value.Year == date.Year)
               .GroupBy(n => new
               {
                   Day = n.OrderDate.Value.Day,
               })
                .Select(n => new RevenueDTo
                {
                    Day = n.Key.Day,
                    NumberInvoice = n.Count(),
                    Money= n.Sum(i=>i.TotalPrice).Value
                })
               .ToList();

            //var NumberFood = db.InvoiceDetails.Where(m => m.InvoiceID == n.Key.InvoiceID)
            //                 .GroupBy(m => m.InvoiceID)
            //                 .Select(m => m.Sum(i => i.NumberFood))
            //                 .FirstOrDefault().Value;
            ThongKeDto tk = new ThongKeDto()
            {
                Invoices = lstIV,
                Posts = lstpost,
                Revenues = invoiceDoanhThu
        };

            return Json(tk, JsonRequestBehavior.AllowGet);
    }

    public ActionResult StoreDetail(Guid StoreID)
    {
        int storeAdmin = 0;
        var foods = db.Foods.Where(n => n.StoreID == StoreID).ToList();
        if (Session["UserName"] != null)
        {
            Store store = db.Stores.SingleOrDefault(n => n.StoreID == StoreID);
            string user = Session["UserName"].ToString();
            Customer cus = db.Customers.SingleOrDefault(n => n.UserName == user);
            if (store != null && store.CustomerID.Equals(cus.CustomerID))
            {
                storeAdmin = 1;
            }
            ViewBag.StoreName = store.StoreName;
            ViewBag.StoreAddress = store.Address;
            ViewBag.CustomerID = cus.CustomerID;
        }
        ViewBag.StoreAdmin = storeAdmin;
        ViewBag.StoreID = StoreID;
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
        store.Status = 0;
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
        if (listStoreSale == null)
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
        ss.KeySale = storeSale.KeySale;
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
        if (storeSale.StartSale <= dateNow && storeSale.StopSale >= dateNow)
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
            Customer cus = (Customer)Session["TaiKhoan"];
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

    public ActionResult ModalDatHang(string StoreID)
    {
        var stID = Guid.Parse(StoreID);
        var store = db.Stores.Where(n => n.StoreID == stID).SingleOrDefault();
        return PartialView(store);
    }

    public ActionResult QuanLyKhachHang(string StoreID, int page, string method)
    {
        var stID = Guid.Parse(StoreID);
        if (method == null)
        {
            Session["Message"] = null;
        }
        var lstInvocie = db.Invoices.Where(m => m.StoreID == stID).ToList();
        List<string> lstID = new List<string>();
        lstInvocie.ForEach(n => lstID.Add(n.CustomerID.ToString()));
        var customers = db.Customers.Where(n => lstID.Contains(n.CustomerID.ToString()))
            .OrderBy(n => n.FullName).ToPagedList(page, pageSize);
        ViewBag.StoreID = StoreID;
        return View(customers);
    }

    //ajax
    [HttpGet]
    public JsonResult getStore(string StoreID)
    {
        var stID = Guid.Parse(StoreID);
        var store = db.Stores.Where(n => n.StoreID == stID).SingleOrDefault();
        if (store == null)
        {
            return Json(-1, JsonRequestBehavior.AllowGet);
        }
        StoreEntity st = new StoreEntity
        {
            StoreID = "" + store.StoreID,
            Manner = store.Manner,
            ServiceCharge = (decimal)store.ServiceCharge,
            ShippingFee = (decimal)store.ShippingFee,
            ConditionShip = (decimal)store.ConditionShip,
            StoreName = store.StoreName,
            StoreType = store.StoreType,
            Address = store.Address
        };
        return Json(st, JsonRequestBehavior.AllowGet);
    }


    [HttpGet]
    public JsonResult appySale(string KeySale, string StoreID)
    {
        var stID = Guid.Parse(StoreID);
        var sale = db.StoreSales.Where(n => n.KeySale == KeySale && n.StoreID == stID && n.StartSale <= DateTime.Now && n.StopSale >= DateTime.Now).SingleOrDefault();
        if (sale == null)
        {
            return Json(-1, JsonRequestBehavior.AllowGet);
        }
        SaleDto slDto = new SaleDto
        {
            StoreID = "" + sale.StoreID,
            SaleNumber = (int)sale.Sale,
            StoreSaleID = sale.StoreSaleID
        };
        return Json(slDto, JsonRequestBehavior.AllowGet);
    }

    [HttpGet]
    public JsonResult getFoodDetail(string FoodID)
    {
        var fdID = Guid.Parse(FoodID);
        var food = db.Foods.Where(n => n.FoodID == fdID).SingleOrDefault();
        List<FoodSizeEntity> foodSizes = new List<FoodSizeEntity>();
        List<FoodTypeEntity> foodTypes = new List<FoodTypeEntity>();
        foreach (var item in food.FoodSizes)
        {
            foodSizes.Add(new FoodSizeEntity()
            {
                FoodID = item.FoodID,
                FoodSizeID = item.FoodSizeID,
                PriceSize = item.PriceSize,
                SizeName = item.SizeName
            });
        }
        foreach (var item in food.FoodTypes)
        {
            foodTypes.Add(new FoodTypeEntity()
            {
                FoodID = item.FoodID,
                FoodTypeID = item.FoodTypeID,
                PriceType = item.PriceType,
                TypeName = item.TypeName
            });
        }
        FoodEntity foodentity = new FoodEntity()
        {
            FoodID = food.FoodID,
            FileID = food.FileID,
            FoodImage = food.FoodImage,
            FoodName = food.FoodName,
            Price = food.Price,
            FoodSizes = foodSizes,
            FoodTypes = foodTypes
        };
        return Json(foodentity, JsonRequestBehavior.AllowGet);
    }
}
}