using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Foody.Models;
using Foody.Models.Dto;
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
            Food food = db.Foods.SingleOrDefault(n => n.FoodID == stdId);
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
                sanpham = new GioHang(FoodID, -1, -1);
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
            List<GioHangDto> listGHDto = new List<GioHangDto>();
            List<GioHangStore> listGH = Session["GioHangStore"] as List<GioHangStore>;
            if (listGH == null)
            {
                return PartialView(listGHDto);
            }

            foreach (var giohang in listGH)
            {
                if (giohang.Orders != null && giohang.Orders.Count > 0)
                {
                    var stID = Guid.Parse(giohang.StoreID);
                    Store store = db.Stores.Where(n => n.StoreID == stID).SingleOrDefault();
                    if (store == null)
                    {
                        return PartialView(listGHDto);
                    }
                    var soPhan = 0;
                    giohang.Orders.ForEach(n => soPhan += n.SoLuong);
                    GioHangDto ghDto = new GioHangDto();
                    ghDto.StoreID = store.StoreID + "";
                    ghDto.StoreName = store.StoreName;
                    ghDto.StoreImage = store.StoreBanner;
                    ghDto.SoPhan = soPhan;
                    listGHDto.Add(ghDto);
                }
            }
            return PartialView(listGHDto);
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

        // GET: GioHang

        // ajax
        public GioHangStore getGioHangStore(string storeID)
        {
            List<GioHangStore> listGH = Session["GioHangStore"] as List<GioHangStore>;
            if (listGH == null)
            {
                listGH = new List<GioHangStore>();
                GioHangStore giohang = new GioHangStore();
                List<GioHang> order = new List<GioHang>();
                giohang.StoreID = storeID;
                giohang.Orders = order;
                listGH.Add(giohang);
                Session["GioHangStore"] = listGH;
                return giohang;
            }
            else if (listGH.Find(n => n.StoreID == storeID) == null)
            {
                GioHangStore giohang = new GioHangStore();
                List<GioHang> order = new List<GioHang>();
                giohang.StoreID = storeID;
                giohang.Orders = order;
                listGH.Add(giohang);
                Session["GioHangStore"] = listGH;
                return giohang;
            }
            GioHangStore giohangStore = listGH.Find(n => n.StoreID == storeID);
            return giohangStore;
        }

        [HttpGet]
        public JsonResult getGioHang(string storeID)
        {
            if (Session["TaiKhoan"] != null)
            {
                GioHangStore giohangStore = getGioHangStore(storeID);
                return Json(giohangStore.Orders, JsonRequestBehavior.AllowGet);
            }
            return Json(-1, JsonRequestBehavior.AllowGet);
        }

        //Thêm giỏ hàng
        [HttpPost]
        public JsonResult AddFood(FoodAddDto foodadd)
        {
            var fdID = Guid.Parse(foodadd.FoodID);
            Food food = db.Foods.SingleOrDefault(n => n.FoodID == fdID);
            if (food == null)
            {
                return Json(null);
            }
            //Lấy ra session giỏ hàng
            GioHangStore gioHangStore = getGioHangStore(food.StoreID + "");
            List<GioHang> lstGioHang = new List<GioHang>();
            if (gioHangStore.Orders != null) { lstGioHang = gioHangStore.Orders; }
            //Kiểm tra sách này đã tồn tại trong session[giohang] chưa
            string sizeName;
            string typeName;
            FoodSize size = db.FoodSizes.Where(n => n.FoodSizeID == foodadd.FoodSizeID).SingleOrDefault();
            FoodType type = db.FoodTypes.Where(n => n.FoodTypeID == foodadd.FoodTypeID).SingleOrDefault();
            GioHang sanpham = lstGioHang.Find(n => n.FoodID.Equals(foodadd.FoodID));
            sizeName = (size == null) ? "" : size.SizeName;
            typeName = (type == null) ? "" : type.TypeName;
            if (sanpham == null || !sanpham.FoodSize.Equals(sizeName) || !sanpham.FoodType.Equals(typeName))
            {
                sanpham = new GioHang(foodadd.FoodID, foodadd.FoodSizeID, foodadd.FoodTypeID, foodadd.SoLuong);
                //Add sản phẩm mới thêm vào list
                lstGioHang.Add(sanpham);
                gioHangStore.Orders = lstGioHang;
                return Json(lstGioHang);
            }
            else
            {
                sanpham.SoLuong += foodadd.SoLuong;
                return Json(lstGioHang);
            }

        }

        [HttpPost]
        public JsonResult DatHang(DatHangDto datHangDto)
        {
            if (Session["TaiKhoan"] != null)
            {
                try
                {
                    Customer cus = (Customer)Session["TaiKhoan"];
                    var stID = Guid.Parse(datHangDto.StoreID);
                    Store store = db.Stores.Where(n => n.StoreID == stID).SingleOrDefault();
                    GioHangStore gioHangStore = getGioHangStore(stID + "");
                    List<GioHang> lstGioHang = new List<GioHang>();
                    if (store != null && gioHangStore.Orders != null)
                    {
                        lstGioHang = gioHangStore.Orders;
                        var tongCong = 0;
                        lstGioHang.ForEach(n => tongCong += (int)n.ThanhTien);
                        var totalPrice = tongCong;
                        var serviceChange = 0;
                        if (tongCong < store.ConditionShip)
                        {
                            totalPrice += (int)store.ServiceCharge;
                            serviceChange = (int)store.ServiceCharge;
                        }
                        var sale = 0;
                        if (datHangDto.StoreSaleID != 0)
                        {
                            StoreSale stSale = db.StoreSales.Where(n => n.StoreSaleID == datHangDto.StoreSaleID && n.StoreID == store.StoreID).SingleOrDefault();
                            if(stSale != null)
                            {
                                sale = totalPrice * (int)stSale.Sale / 100;
                                totalPrice = totalPrice - sale;
                            }
                        }
                        var shippingFee = 0;
                        if (datHangDto.Distance > 0)
                        {
                            shippingFee = (int)(datHangDto.Distance * store.ShippingFee);
                        }
                        lstGioHang = gioHangStore.Orders;
                        Invoice iv = new Invoice()
                        {
                            InvoiceID = Guid.NewGuid(),
                            OrderDate = DateTime.Now,
                            DeliveryDate = DateTime.Parse(datHangDto.DateDelivery + " " + datHangDto.TimeDelivery),
                            AddressDelivery = datHangDto.CustomerAddress,
                            CustomerID = cus.CustomerID,
                            CustomerName = datHangDto.CustomerName,
                            CustomerPhone = datHangDto.CustomerPhone,
                            StoreID = stID,
                            ServiceChange = serviceChange,
                            Sale = sale,
                            ShippingFee = shippingFee,
                            TotalPrice = totalPrice,
                            Status = 0
                        };
                        if (ModelState.IsValid)
                        {
                            //Thực hiện cập nhận trong model
                            db.Entry(iv).State = System.Data.Entity.EntityState.Added;
                            db.SaveChanges();
                        }

                        foreach (var gh in lstGioHang)
                        {
                            var foodID = Guid.Parse(gh.FoodID);
                            InvoiceDetail ivDT = new InvoiceDetail()
                            {
                                InvoiceID = iv.InvoiceID,
                                FoodID = foodID,
                                Size = gh.FoodSize,
                                PriceSize = (decimal)gh.PriceSize,
                                Type = gh.FoodType,
                                PriceType = (Decimal)gh.PriceType,
                                NumberFood = gh.SoLuong,
                            };
                            if (ModelState.IsValid)
                            {
                                //Thực hiện cập nhận trong model
                                db.Entry(ivDT).State = System.Data.Entity.EntityState.Added;
                                db.SaveChanges();
                            }
                        }
                        return Json(true);
                    }
                }
                catch
                {
                    return Json(false);
                }
            }
            return Json(false);
        }


        [HttpPost]
        public JsonResult deleteCart(string storeID)
        {
            if (Session["TaiKhoan"] != null)
            {
                List<GioHangStore> listGH = Session["GioHangStore"] as List<GioHangStore>;
                if (listGH != null)
                {
                    listGH.RemoveAll(n => n.StoreID.Equals(storeID));
                }
                return Json(true);
            }
            return Json(false);
        }

    }
}