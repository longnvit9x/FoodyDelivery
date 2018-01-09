using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Foody.Models;
using Foody.Models.Entity;

namespace Foody.Areas.Administrator.Controllers
{
    public class AdminStoreController : Controller
    {
        private FoodDeliveryEntities db;
        public AdminStoreController()
        {
            db = new FoodDeliveryEntities();
        }
        // GET: Administrator/AdminStore
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult QuanlyCuaHang()
        {
            List<Store> lst = new List<Store>();
            lst = db.Stores.OrderBy(n => n.Status).ToList();
            return View(lst);
        }

        [HttpGet]
        public JsonResult getAll()
        {
            List<StoreEntity> list = new List<StoreEntity>();
            var stores = db.Stores.OrderBy(n => n.Status).ToList();
            foreach (var obj in stores)
            {
                StoreEntity entity = new StoreEntity();
                entity.StoreID = "" + obj.StoreID;
                entity.Status = (Int16)obj.Status;
                list.Add(entity);
            }
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        [HttpPut]
        public JsonResult SuaSTTStore(string StoreID, int Status)
        {
            try
            {
                var stID = Guid.Parse(StoreID);
                var st = db.Stores.Where(n => n.StoreID == stID).SingleOrDefault();
                if (st == null)
                {
                    //Trả về trang báo lỗi 
                    Response.StatusCode = 404;
                    return null;
                }
                st.Status = Status;
                //Thực hiện cập nhận trong model
                db.Entry(st).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return Json(true);
            }
            catch
            {
                return Json(false);
            }
        }
    }
}