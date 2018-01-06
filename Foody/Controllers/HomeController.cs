using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Foody.Models;
using PagedList;
using System.IO;
using System.Drawing;

namespace Foody.Controllers
{
    public class HomeController : Controller
    {
        private FoodDeliveryEntities db;
        int pageSize = 9;
        string sThongBao = "Không tìm kết quả nào phù hợp";
        public HomeController()
        {
            db = new FoodDeliveryEntities();
        }


        public ActionResult Index(int page = 1)
        {
            var model = db.Stores.OrderBy(x => x.StoreName).ToPagedList(page, pageSize);
            return View(model);
        }

        [HttpPost]
        public ActionResult TimKiem(FormCollection f = null, int page = 1)
        {
            string sStoreName = "";
            string sAddress = "";
            string sStoreType = "";
            if (f != null)
            {
                sStoreName = f["txtStoreName"].ToString();
                sAddress = f["cbxAddress"].ToString();
                sStoreType = f["cbxStoreType"].ToString();
            }
            ViewBag.StoreName = sStoreName;
            ViewBag.Addres = sAddress;
            ViewBag.StoreType = sStoreType;
            List<Store> lstKQTK = db.Stores.Where(n => n.StoreName.Contains(sStoreName)
            && n.Address.Contains(sAddress)
            && n.StoreType.Contains(sStoreType)).ToList();
            if (lstKQTK.Count == 0)
            {
                ViewBag.ThongBao = sThongBao;
                return View(db.Stores.OrderBy(n => n.StoreName).ToPagedList(page, pageSize));
            }
            ViewBag.ThongBao = "Đã tìm thấy " + lstKQTK.Count + " kết quả!";
            return View(lstKQTK.OrderBy(n => n.StoreName).ToPagedList(page, pageSize));
        }

        public ActionResult TimKiem(int page = 1, string storeType = "")
        {
            string sStoreType = storeType;
            ViewBag.StoreType = sStoreType;
            List<Store> lstKQTK = db.Stores.Where(n => n.StoreType.Contains(sStoreType)).ToList();
            if (lstKQTK.Count == 0)
            {
                ViewBag.ThongBao = sThongBao;
                return View(db.Stores.OrderBy(n => n.StoreName).ToPagedList(page, pageSize));
            }
            ViewBag.ThongBao = "Đã tìm thấy " + lstKQTK.Count + " kết quả!";
            return View(lstKQTK.OrderBy(n => n.StoreName).ToPagedList(page, pageSize));
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult SlideHome()
        {
            var model = db.Stores.OrderBy(x => x.StoreName).Take(4).ToList();
            return PartialView(model);
        }
        public ActionResult MenuTop()
        {
            if (Session["FullName"] != null)
            {
                ViewBag.FullName = Session["FullName"].ToString();
                var cusID = Guid.Parse(Session["CustomerID"].ToString());
                var cus = (Customer)Session["TaiKhoan"];
                FileData fileData = db.FileDatas.FirstOrDefault(p => p.FileID == cus.FileID);

                if (fileData != null && fileData.URL!=null)
                {
                    ViewBag.Avatar = fileData.URL;
                }
                else
                {
                    ViewBag.Avatar = cus.Avatar;
                }
                ViewBag.CustomerImage = cus.Avatar;
                List<Store> lstKQTK = db.Stores.Where(n => n.CustomerID == cusID).ToList();
                List<Post> lstPost = db.Posts.Where(n => n.CustomerID == cusID && n.Status==0).OrderBy(n=> n.PostTime).ToList();
                ViewBag.PostNew = lstPost;
                return PartialView(lstKQTK);
            }
            return PartialView();
        }

        public ActionResult CuaHangKhuyenMai()
        {
            var model = db.Stores
                .Where(s => db.StoreSales
                            .Where(ss => ss.StartSale <= DateTime.Now)
                            .Select(ss => ss.StoreID)
                            .Contains(s.StoreID)
                         )
                .OrderBy(x => x.StoreName)
                .ToList();
            return PartialView(model);
        }

        public ActionResult CuaHangDuocDanhGiaCao()
        {
            var model = db.Stores
                .OrderByDescending(x => x.Rating)
                .ToList();
            return PartialView(model);
        }

        public ActionResult MenuLeft()
        {
            return PartialView();
        }
    }
}