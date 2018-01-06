using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Foody.Models;
using PagedList;

namespace Foody.Controllers
{
    public class CategoryController : Controller
    {
        int pageSize = 9;
        private FoodDeliveryEntities db;
        public CategoryController()
        {
            db = new FoodDeliveryEntities();
        }

        public ActionResult QuanLyDanhMucMonAn(string StoreID, int page, string method)
        {
            if (method == null)
            {
                Session["Message"] = null;
            }
            var stID = Guid.Parse(StoreID);
            var category = db.Categories.Where(n => n.StoreID == stID).OrderBy(x => x.CategoryName).ToPagedList(page, pageSize);
            if (category == null)
            {
                //Trả về trang báo lỗi 
                Response.StatusCode = 404;
                return null;
            }
            ViewBag.StoreID = StoreID;
            return View(category);
        }

        public ActionResult ThemDanhMuc(string StoreID)
        {
            ViewBag.StoreID = StoreID;
            return View();
        }

        [HttpPost]
        public ActionResult ThemDanhMuc(Category category,string StoreID)
        {
            category.CategoryID = Guid.NewGuid();
            category.StoreID = Guid.Parse(StoreID);
            if (ModelState.IsValid)
            {
                //Thực hiện cập nhận trong model
                db.Entry(category).State = System.Data.Entity.EntityState.Added;
                db.SaveChanges();
                Session["Message"] = category.CategoryName + " thêm thành công";
                return RedirectToAction("QuanLyDanhMucMonAn",new { StoreID= StoreID, page=1, method = "ThemMoi"});
            }
            ViewBag.StoreID = StoreID;
            return View(category);
        }

        public ActionResult SuaDanhMuc(string CategoryID,string StoreID)
        {
            var ctID = Guid.Parse(CategoryID);
            var category = db.Categories.Where(n => n.CategoryID == ctID).SingleOrDefault();
            if(category== null)
            {
                //Trả về trang báo lỗi 
                Response.StatusCode = 404;
                return null;
            }
            ViewBag.StoreID = StoreID;
            return View(category);
        }

        [HttpPost]
        public ActionResult SuaDanhMuc(Category category, string StoreID)
        {
            var ct = db.Categories.Where(n => n.CategoryID == category.CategoryID).SingleOrDefault();
            if (ct == null)
            {
                //Trả về trang báo lỗi 
                Response.StatusCode = 404;
                return null;
            }
            ct.CategoryName = category.CategoryName;
            if (ModelState.IsValid)
            {
                //Thực hiện cập nhận trong model
                db.Entry(ct).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                Session["Message"] = category.CategoryName + " sửa thành công";
                return RedirectToAction("QuanLyDanhMucMonAn", new { StoreID = StoreID, page = 1 , method = "Sua" });
            }
            ViewBag.StoreID = StoreID;
            return View(category);
        }

        public ActionResult XoaDanhMuc(string CategoryID, string StoreID)
        {
            var ctID = Guid.Parse(CategoryID);
            var category = db.Categories.Where(n => n.CategoryID == ctID).SingleOrDefault();
            if (category == null)
            {
                //Trả về trang báo lỗi 
                Response.StatusCode = 404;
                return null;
            }
            List<Food> lstFood = db.Foods.Where(n => n.CategoryID == ctID).ToList();
            if(lstFood!=null && lstFood.Count >0)
            {
                Session["Message"] = category.CategoryName + " Không thể xóa vì tồn tại các món ăn thuộc danh mục này";
                return RedirectToAction("QuanLyDanhMucMonAn", new { StoreID = StoreID, page = 1, method = "Xoa" });
            }
            db.Entry(category).State = System.Data.Entity.EntityState.Deleted;
            db.SaveChanges();
            Session["Message"] = category.CategoryName + " Xóa thành công!";
            ViewBag.StoreID = StoreID;
            return RedirectToAction("QuanLyDanhMucMonAn", new { StoreID = StoreID, page = 1, method = "Xoa" });
        }
    }
}