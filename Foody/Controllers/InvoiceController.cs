using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Foody.Models;
using Foody.Models.Entity;
using PagedList;

namespace Foody.Controllers
{
    public class InvoiceController : Controller
    {
        int pageSize = 9;
        private FoodDeliveryEntities db;
        public InvoiceController()
        {
            db = new FoodDeliveryEntities();
        }

        public ActionResult QuanLyHoaDon(string StoreID, int page, string method)
        {
            if (method == null)
            {
                Session["Message"] = null;
            }
            var stID = Guid.Parse(StoreID);
            var invoices = db.Invoices.Where(n => n.StoreID == stID)
                .OrderBy(x => x.Status)
                .ToPagedList(page, pageSize);
            if (invoices == null)
            {
                //Trả về trang báo lỗi 
                Response.StatusCode = 404;
                return null;
            }
            ViewBag.StoreID = StoreID;
            return View(invoices);
        }

        public ActionResult SuaTTHoaDon(string InvoiceID, int status, string StoreID)
        {
            var ivID = Guid.Parse(InvoiceID);
            var f = db.Invoices.Where(n => n.InvoiceID == ivID).SingleOrDefault();
            if (f == null)
            {
                //Trả về trang báo lỗi 
                Response.StatusCode = 404;
                return null;
            }
            f.Status = status;
            if (ModelState.IsValid)
            {
                //Thực hiện cập nhận trong model
                db.Entry(f).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                Session["Message"] = f.InvoiceID + " cập nhật thành công";
                return RedirectToAction("QuanLyHoaDon", "Invoice", new { StoreID = StoreID, page = 1, method = "Sua" });
            }
            Session["Message"] = f.InvoiceID + " cập nhật thất bại";
            return RedirectToAction("QuanLyHoaDon", "Invoice", new { StoreID = StoreID, page = 1, method = "Sua" });
        }

        public ActionResult InvoiceDetail(Guid StoreID, Guid InvoiceID, int page)
        {

            var invoiceDetails = db.InvoiceDetails.Where(n => n.InvoiceID == InvoiceID).OrderBy(x => x.FoodID).ToPagedList(page, pageSize);
            if (invoiceDetails == null)
            {
                //Trả về trang báo lỗi 
                Response.StatusCode = 404;
                return null;
            }
            ViewBag.StoreID = StoreID;
            ViewBag.InvoiceId = InvoiceID;
            return View(invoiceDetails);
        }

        // Ajax
        [HttpGet]
        public JsonResult getAll(string StoreID)
        {
            try
            {
                var stID = Guid.Parse(StoreID);
                List<InvoiceEntity> list = new List<InvoiceEntity>();
                var invoices = db.Invoices.Where(n => n.StoreID == stID).OrderBy(x => x.CustomerName).ToList();
                foreach (var obj in invoices)
                {
                    InvoiceEntity entity = new InvoiceEntity
                    {
                        InvoiceID = "" + obj.InvoiceID,
                        OrderDate = "" + obj.OrderDate,
                        DeliveryDate = "" + obj.DeliveryDate,
                        Sale = (decimal)obj.Sale,
                        ServiceChange = (decimal)obj.ServiceChange,
                        ShippingFee = (decimal)obj.ShippingFee,
                        TotalPrice = (decimal)obj.TotalPrice,
                        StoreID = "" + obj.StoreID,
                        CustomerID = "" + obj.CustomerID,
                        AddressDelivery = obj.AddressDelivery,
                        CustomerName = obj.CustomerName,
                        CustomerPhone = obj.CustomerPhone,
                        Status = (int)obj.Status
                    };
                    list.Add(entity);
                }
                return Json(list, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return null;
            }
        }

        [HttpPut]
        public JsonResult SuaSTTHoaDon(string InvoiceID, int Status)
        {
            try
            {
                var ivID = Guid.Parse(InvoiceID);
                var f = db.Invoices.Where(n => n.InvoiceID == ivID).SingleOrDefault();
                if (f == null)
                {
                    //Trả về trang báo lỗi 
                    Response.StatusCode = 404;
                    return null;
                }
                f.Status = Status;
                //Thực hiện cập nhận trong model
                db.Entry(f).State = System.Data.Entity.EntityState.Modified;
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