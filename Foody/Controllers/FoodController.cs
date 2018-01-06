using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Foody.Models;
using PagedList;

namespace Foody.Controllers
{
    public class FoodController : Controller
    {
        int pageSize = 9;
        private FoodDeliveryEntities db;
        public FoodController()
        {
            db = new FoodDeliveryEntities();
        }

        public ActionResult QuanLyMonAn(Guid StoreID, int page, string method)
        {
            if (method == null)
            {
                Session["Message"] = null;
            }
            var foods = db.Foods.Where(n => n.StoreID == StoreID).OrderBy(x => x.FoodName).ToPagedList(page, pageSize);
            if (foods == null)
            {
                //Trả về trang báo lỗi 
                Response.StatusCode = 404;
                return null;
            }
            ViewBag.StoreID = StoreID;
            return View(foods);
        }

        public ActionResult ThemMonAn(string StoreID)
        {
            var stID = Guid.Parse(StoreID);
            ViewBag.CategoryID = new SelectList(db.Categories.Where(n => n.StoreID == stID)
                .OrderBy(n => n.CategoryName).ToList(), "CategoryID", "CategoryName");
            ViewBag.StoreID = StoreID;
            return View();
        }

        [HttpPost]
        public ActionResult ThemMonAn(Food food, string StoreID)
        {
            food.FoodID = Guid.NewGuid();
            food.StoreID = Guid.Parse(StoreID);
            if (ModelState.IsValid)
            {
                //Thực hiện cập nhận trong model
                db.Entry(food).State = System.Data.Entity.EntityState.Added;
                db.SaveChanges();
                Session["Message"] = food.FoodName + " thêm thành công";
               return RedirectToAction("QuanLyMonAn", new { StoreID = StoreID, page = 1, method = "ThemMoi" });
            }
            ViewBag.CategoryID = new SelectList(db.Categories.Where(n => n.StoreID == food.StoreID)
              .OrderBy(n => n.CategoryName).ToList(), "CategoryID", "CategoryName",food.CategoryID);
            ViewBag.StoreID = StoreID;
            return View(food);
        }

        public ActionResult SuaMonAn(string FoodID, string StoreID)
        {
            var fID = Guid.Parse(FoodID);
            var stID = Guid.Parse(StoreID);
            var food = db.Foods.Where(n => n.FoodID == fID).SingleOrDefault();
            if (food == null)
            {
                //Trả về trang báo lỗi 
                Response.StatusCode = 404;
                return null;
            }
            ViewBag.CategoryID = new SelectList(db.Categories.Where(n => n.StoreID == stID).OrderBy(n => n.CategoryName).ToList(), "CategoryID", "CategoryName",food.CategoryID);
            ViewBag.StoreID = StoreID;
            return View(food);
        }

        [HttpPost]
        public ActionResult SuaMonAn(Food food,string StoreID)
        {
            var fd = db.Foods.Where(n => n.FoodID == food.FoodID).SingleOrDefault();
            if (fd == null)
            {
                //Trả về trang báo lỗi 
                Response.StatusCode = 404;
                return null;
            }
            fd.FoodName = food.FoodName;
            fd.Price = food.Price;
            fd.FoodImage = food.FoodImage;
            fd.CategoryID = food.CategoryID;
            if (ModelState.IsValid)
            {
                //Thực hiện cập nhận trong model
                db.Entry(fd).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                Session["Message"] = fd.FoodName + " sửa thành công";
                return RedirectToAction("QuanLyMonAn", new { StoreID = StoreID, page = 1, method = "Sua" });
            }
            ViewBag.CategoryID = new SelectList(db.Categories.Where(n => n.StoreID == food.StoreID)
             .OrderBy(n => n.CategoryName).ToList(), "CategoryID", "CategoryName", food.CategoryID);
            ViewBag.StoreID = StoreID;
            return View(food);
        }

        public ActionResult XoaMonAn(string FoodID, string StoreID)
        {
            var fID = Guid.Parse(FoodID);
            var stID = Guid.Parse(StoreID);
            var food = db.Foods.Where(n => n.FoodID == fID).SingleOrDefault();
            if (food == null)
            {
                //Trả về trang báo lỗi 
                Response.StatusCode = 404;
                return null;
            }
            var lstFoodType= db.FoodTypes.Where(n => n.FoodID == fID).ToList();
            if(lstFoodType!= null && lstFoodType.Count>0)
            {
                Session["Message"] = food.FoodName + " không thể xóa vì tồn tại loại của món ăn này";
                return RedirectToAction("QuanLyMonAn", new { StoreID = StoreID, page = 1, method = "Xoa" });
            }
            var lstFoodSize = db.FoodSizes.Where(n => n.FoodID == fID).ToList();
            if (lstFoodSize != null && lstFoodSize.Count > 0)
            {
                Session["Message"] = food.FoodName + " không thể xóa vì tồn tại size của món ăn này";
                return RedirectToAction("QuanLyMonAn", new { StoreID = StoreID, page = 1, method = "Xoa" });
            }

            db.Entry(food).State = System.Data.Entity.EntityState.Deleted;
            db.SaveChanges();
            Session["Message"] = food.FoodName + " xóa thành công";
            ViewBag.StoreID = StoreID;
            return RedirectToAction("QuanLyMonAn", new { StoreID = StoreID, page = 1, method = "Xoa" });
        }

        // FoodSize
        public ActionResult QuanLySizeMonAn(string StoreID, int page, string method)
        {
            var stID = Guid.Parse(StoreID);
            if (method == null)
            {
                Session["Message"] = null;
            }
            
            var lstFoodID = db.Foods.Where(n => n.StoreID == stID).Select(n=> n.FoodID).ToList();
            List<string> lstID = new List<string>();
            lstFoodID.ForEach(n => lstID.Add(n.ToString()));
            var lstFoodSize = db.FoodSizes.Where(n => lstID.Contains(n.FoodID.ToString()))
            .OrderBy(n => n.FoodID).ToPagedList(page, pageSize);
            if (lstFoodSize == null)
            {
                //Trả về trang báo lỗi 
                Response.StatusCode = 404;
                return null;
            }
            ViewBag.StoreID = StoreID;
            return View(lstFoodSize);
        }

        public ActionResult ThemSizeMonAn(string StoreID)
        {
            var stID = Guid.Parse(StoreID);
            ViewBag.FoodID = new SelectList(db.Foods.Where(n => n.StoreID == stID)
                .OrderBy(n => n.FoodName).ToList(), "FoodID", "FoodName");
            ViewBag.StoreID = StoreID;
            return View();
        }

        [HttpPost]
        public ActionResult ThemSizeMonAn(FoodSize foodsize, string StoreID)
        {
            var stID = Guid.Parse(StoreID);
            if (ModelState.IsValid)
            {
                db.Entry(foodsize).State = System.Data.Entity.EntityState.Added;
                db.SaveChanges();
                Session["Message"] = foodsize.SizeName + " thêm thành công";
                return RedirectToAction("QuanLySizeMonAn", new { StoreID = StoreID, page = 1, method = "ThemMoi" });
            }
            ViewBag.FoodID = new SelectList(db.Foods.Where(n => n.StoreID == stID)
               .OrderBy(n => n.FoodName).ToList(), "FoodID", "FoodName",foodsize.FoodID);
            ViewBag.StoreID = StoreID;
            return View(foodsize);
        }

        public ActionResult SuaSizeMonAn(int FoodSizeID, string StoreID)
        {
            var stID = Guid.Parse(StoreID);
            var foodSize = db.FoodSizes.Where(n => n.FoodSizeID == FoodSizeID).SingleOrDefault();
            if (foodSize == null)
            {
                //Trả về trang báo lỗi 
                Response.StatusCode = 404;
                return null;
            }
            ViewBag.FoodID = new SelectList(db.Foods.Where(n => n.StoreID == stID)
                .OrderBy(n => n.FoodName).ToList(), "FoodID", "FoodName", foodSize.FoodID);
            ViewBag.StoreID = StoreID;
            return View(foodSize);
        }

        [HttpPost]
        public ActionResult SuaSizeMonAn(FoodSize foodsize, string StoreID)
        {
            var stID = Guid.Parse(StoreID);
            var fs = db.FoodSizes.Where(n => n.FoodSizeID == foodsize.FoodSizeID).SingleOrDefault();
            if (fs == null)
            {
                //Trả về trang báo lỗi 
                Response.StatusCode = 404;
                return null;
            }
            fs.SizeName = foodsize.SizeName;
            fs.PriceSize = foodsize.PriceSize;
            fs.FoodID = foodsize.FoodID;
            if (ModelState.IsValid)
            {
                //Thực hiện cập nhận trong model
                db.Entry(fs).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                Session["Message"] = fs.SizeName + " sửa thành công";
                return RedirectToAction("QuanLySizeMonAn", new { StoreID = StoreID, page = 1, method = "Sua" });
            }
            ViewBag.FoodID = new SelectList(db.Foods.Where(n => n.StoreID == stID)
                .OrderBy(n => n.FoodName).ToList(), "FoodID", "FoodName", foodsize.FoodID);
            ViewBag.StoreID = StoreID;
            return View(foodsize);
        }

        public ActionResult XoaSizeMonAn(int FoodSizeID, string StoreID)
        {
            var stID = Guid.Parse(StoreID);
            var foodSize = db.FoodSizes.Where(n => n.FoodSizeID == FoodSizeID).SingleOrDefault();
            if (foodSize == null)
            {
                //Trả về trang báo lỗi 
                Response.StatusCode = 404;
                return null;
            }
            db.Entry(foodSize).State = System.Data.Entity.EntityState.Deleted;
            db.SaveChanges();
            Session["Message"] = foodSize.SizeName + " xóa thành công";
            ViewBag.StoreID = StoreID;
            return RedirectToAction("QuanLySizeMonAn", new { StoreID = StoreID, page = 1, method = "Xoa" });
        }

        // FoodType
        public ActionResult QuanLyTypeMonAn(string StoreID, int page, string method)
        {
            var stID = Guid.Parse(StoreID);
            if (method == null)
            {
                Session["Message"] = null;
            }

            var lstFoodID = db.Foods.Where(n => n.StoreID == stID).Select(n => n.FoodID).ToList();
            List<string> lstID = new List<string>();
            lstFoodID.ForEach(n => lstID.Add(n.ToString()));
            var lstFoodType = db.FoodTypes.Where(n => lstID.Contains(n.FoodID.ToString()))
            .OrderBy(n => n.FoodID).ToPagedList(page, pageSize);
            if (lstFoodType == null)
            {
                //Trả về trang báo lỗi 
                Response.StatusCode = 404;
                return null;
            }
            ViewBag.StoreID = StoreID;
            return View(lstFoodType);
        }

        public ActionResult ThemTypeMonAn(string StoreID)
        {
            var stID = Guid.Parse(StoreID);
            ViewBag.FoodID = new SelectList(db.Foods.Where(n => n.StoreID == stID)
                .OrderBy(n => n.FoodName).ToList(), "FoodID", "FoodName");
            ViewBag.StoreID = StoreID;
            return View();
        }

        [HttpPost]
        public ActionResult ThemTypeMonAn(FoodType foodtype, string StoreID)
        {
            var stID = Guid.Parse(StoreID);
            if (ModelState.IsValid)
            {
                db.Entry(foodtype).State = System.Data.Entity.EntityState.Added;
                db.SaveChanges();
                Session["Message"] = foodtype.TypeName + " thêm thành công";
                return RedirectToAction("QuanLyTypeMonAn", new { StoreID = StoreID, page = 1, method = "ThemMoi" });
            }
            ViewBag.FoodID = new SelectList(db.Foods.Where(n => n.StoreID == stID)
               .OrderBy(n => n.FoodName).ToList(), "FoodID", "FoodName", foodtype.FoodID);
            ViewBag.StoreID = StoreID;
            return View(foodtype);
        }

        public ActionResult SuaTypeMonAn(int FoodTypeID, string StoreID)
        {
            var stID = Guid.Parse(StoreID);
            var foodType = db.FoodTypes.Where(n => n.FoodTypeID == FoodTypeID).SingleOrDefault();
            if (foodType == null)
            {
                //Trả về trang báo lỗi 
                Response.StatusCode = 404;
                return null;
            }
            ViewBag.FoodID = new SelectList(db.Foods.Where(n => n.StoreID == stID)
                .OrderBy(n => n.FoodName).ToList(), "FoodID", "FoodName", foodType.FoodID);
            ViewBag.StoreID = StoreID;
            return View(foodType);
        }

        [HttpPost]
        public ActionResult SuaTypeMonAn(FoodType foodType, string StoreID)
        {
            var stID = Guid.Parse(StoreID);
            var ft = db.FoodTypes.Where(n => n.FoodTypeID == foodType.FoodTypeID).SingleOrDefault();
            if (ft == null)
            {
                //Trả về trang báo lỗi 
                Response.StatusCode = 404;
                return null;
            }
            ft.TypeName = foodType.TypeName;
            ft.PriceType = foodType.PriceType;
            ft.FoodID = foodType.FoodID;
            if (ModelState.IsValid)
            {
                //Thực hiện cập nhận trong model
                db.Entry(ft).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                Session["Message"] = ft.TypeName + " sửa thành công";
                return RedirectToAction("QuanLyTypeMonAn", new { StoreID = StoreID, page = 1, method = "Sua" });
            }
            ViewBag.FoodID = new SelectList(db.Foods.Where(n => n.StoreID == stID)
                 .OrderBy(n => n.FoodName).ToList(), "FoodID", "FoodName", foodType.FoodID);
            ViewBag.StoreID = StoreID;
            return View(foodType);
        }

        public ActionResult XoaTypeMonAn(int FoodTypeID, string StoreID)
        {
            var stID = Guid.Parse(StoreID);
            var foodType = db.FoodTypes.Where(n => n.FoodTypeID == FoodTypeID).SingleOrDefault();
            if (foodType == null)
            {
                //Trả về trang báo lỗi 
                Response.StatusCode = 404;
                return null;
            }
            db.Entry(foodType).State = System.Data.Entity.EntityState.Deleted;
            db.SaveChanges();
            Session["Message"] = foodType.TypeName + " xóa thành công";
            ViewBag.StoreID = StoreID;
            return RedirectToAction("QuanLyTypeMonAn", new { StoreID = StoreID, page = 1, method = "Xoa" });
        }
    }
}