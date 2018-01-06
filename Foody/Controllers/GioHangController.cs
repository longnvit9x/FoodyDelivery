using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Foody.Models;
namespace Foody.Controllers
{
    public class GioHangController : Controller
    {
        FoodDeliveryEntities db = new FoodDeliveryEntities();
        // GET: GioHang
        public List<GioHang> LayGioHang()
        {
            List<GioHang> lstGioHang = Session["GioHang"] as List<GioHang>;
            if (lstGioHang == null)
            {
                lstGioHang = new List<GioHang>();
                Session["GioHang"] = lstGioHang;
            }
            return lstGioHang;
        }

        //Thêm giỏ hàng
        public ActionResult ThemGioHang(string FoodID, string strURL)
        {
            var stdId = Guid.Parse(FoodID);
            Food food = db.Foods.SingleOrDefault(n => n.FoodID== stdId);
            if (food == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            //Lấy ra session giỏ hàng
            List<GioHang> lstGioHang = LayGioHang();
            //Kiểm tra sách này đã tồn tại trong session[giohang] chưa
            GioHang sanpham = lstGioHang.Find(n => n.FoodID.Equals(FoodID));
            if (sanpham == null)
            {
                sanpham = new GioHang(FoodID);
                //Add sản phẩm mới thêm vào list
                lstGioHang.Add(sanpham);
                return Redirect(strURL);
            }
            else
            {
                sanpham.SoLuong++;
                return Redirect(strURL);
            }
        }
        //Cập nhật giỏ hàng 
        public ActionResult CapNhatGioHang(string FoodID, FormCollection f)
        {
            //Kiểm tra masp
            Food food = db.Foods.SingleOrDefault(n => n.FoodID.Equals(FoodID));
            //Nếu get sai masp thì sẽ trả về trang lỗi 404
            if (food == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            //Lấy giỏ hàng ra từ session
            List<GioHang> lstGioHang = LayGioHang();
            //Kiểm tra sp có tồn tại trong session["GioHang"]
            GioHang sanpham = lstGioHang.SingleOrDefault(n => n.FoodID == FoodID);
            //Nếu mà tồn tại thì chúng ta cho sửa số lượng
            if (sanpham != null)
            {
                sanpham.SoLuong = int.Parse(f["txtSoLuong"].ToString());

            }
            return RedirectToAction("GioHang");
        }
        //Xóa giỏ hàng
        public ActionResult XoaGioHang(string FoodID)
        {
            //Kiểm tra masp
            Food food = db.Foods.SingleOrDefault(n => n.FoodID.Equals(FoodID));
            //Nếu get sai masp thì sẽ trả về trang lỗi 404
            if (food == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            //Lấy giỏ hàng ra từ session
            List<GioHang> lstGioHang = LayGioHang();
            GioHang sanpham = lstGioHang.SingleOrDefault(n => n.FoodID.Equals(FoodID));
            //Nếu mà tồn tại thì chúng ta cho sửa số lượng
            if (sanpham != null)
            {
                lstGioHang.RemoveAll(n => n.FoodID.Equals(FoodID));

            }
            if (lstGioHang.Count == 0)
            {
                return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("GioHang");
        }

        //Xây dựng trang giỏ hàng
        public ActionResult GioHang()
        {
            if (Session["GioHang"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            List<GioHang> lstGioHang = LayGioHang();
            return View(lstGioHang);
        }
        //Tính tổng số lượng và tổng tiền
        //Tính tổng số lượng
        private int TongSoLuong()
        {
            int iTongSoLuong = 0;
            List<GioHang> lstGioHang = Session["GioHang"] as List<GioHang>;
            if (lstGioHang != null)
            {
                iTongSoLuong = lstGioHang.Sum(n => n.SoLuong);
            }
            return iTongSoLuong;
        }
        //Tính tổng thành tiền
        private double TongTien()
        {
            double dTongTien = 0;
            List<GioHang> lstGioHang = Session["GioHang"] as List<GioHang>;
            if (lstGioHang != null)
            {
                dTongTien = lstGioHang.Sum(n => n.ThanhTien);
            }
            return dTongTien;
        }
        //tạo partial giỏ hàng
        public ActionResult GioHangPartial()
        {
            if (TongSoLuong() == 0)
            {
                return PartialView();
            }
            ViewBag.TongSoLuong = TongSoLuong();
            ViewBag.TongTien = TongTien();
            return PartialView();
        }
        //Xây dựng 1 view cho người dùng chỉnh sửa giỏ hàng
        public ActionResult SuaGioHang()
        {
            if (Session["GioHang"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            List<GioHang> lstGioHang = LayGioHang();
            return View(lstGioHang);

        }

        //#region Đặt hàng
        ////Xây dựng chức năng đặt hàng
        //[HttpPost]
        //public ActionResult DatHang()
        //{
        //    //Kiểm tra đăng đăng nhập
        //    if (Session["TaiKhoan"] == null || Session["TaiKhoan"].ToString() == "")
        //    {
        //        return RedirectToAction("DangNhap", "NGuoiDung");
        //    }
        //    //Kiểm tra giỏ hàng
        //    if (Session["GioHang"] == null)
        //    {
        //        RedirectToAction("Index", "Home");
        //    }
        //    //Thêm đơn hàng
        //    DonHang ddh = new DonHang();
        //    KhachHang kh = (KhachHang)Session["TaiKhoan"];
        //    List<GioHang> gh = LayGioHang();
        //    ddh.MaKH = kh.MaKH;
        //    ddh.NgayDat = DateTime.Now;
        //    db.DonHangs.Add(ddh);
        //    db.SaveChanges();
        //    //Thêm chi tiết đơn hàng
        //    foreach (var item in gh)
        //    {
        //        ChiTietDonHang ctDH = new ChiTietDonHang();
        //        ctDH.MaDonHang = ddh.MaDonHang;
        //        ctDH.MaSach = item.iMaSach;
        //        ctDH.SoLuong = item.iSoLuong;
        //        ctDH.DonGia = (decimal)item.dDonGia;
        //        db.ChiTietDonHangs.Add(ctDH);
        //    }
        //    db.SaveChanges();
        //    return RedirectToAction("Index", "Home");
        //}
        //#endregion
    }
}