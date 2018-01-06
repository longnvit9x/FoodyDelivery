using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
namespace Foody.Models
{
    [MetadataTypeAttribute(typeof(InvoiceMetadata))]
    public partial class Invoice
    {
        internal sealed class InvoiceMetadata
        {
            [Display(Name = "Mã hóa đơn")]
            public System.Guid InvoiceID { get; set; }
            [Display(Name = "Ngày đặt hàng")]
            [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm:ss}", ApplyFormatInEditMode = true)]
            public Nullable<System.DateTime> OrderDate { get; set; }
            [Display(Name = "Ngày giao hàng")]
            [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm:ss}", ApplyFormatInEditMode = true)]
            public Nullable<System.DateTime> DeliveryDate { get; set; }
            [Display(Name = "Họ tên người đặt")]
            public string CustomerName { get; set; }
            [Display(Name = "Số điện thoại người đặt")]
            public string CustomerPhone { get; set; }
            [Display(Name = "Địa chỉ giao hàng")]
            public string AddressDelivery { get; set; }
            [Display(Name = "Số tiền khuyến mãi")]
            [DisplayFormat(DataFormatString = "{0:0,000}", ApplyFormatInEditMode = true)]
            public Nullable<decimal> Sale { get; set; }
            [Display(Name = "Phí vận chuyển")]
            [DisplayFormat(DataFormatString = "{0:0,000}", ApplyFormatInEditMode = true)]
            public Nullable<decimal> ShippingFee { get; set; }
            [Display(Name = "Phí dịch vụ")]
            [DisplayFormat(DataFormatString = "{0:0,000}", ApplyFormatInEditMode = true)]
            public Nullable<decimal> ServiceChange { get; set; }
            [DisplayFormat(DataFormatString = "{0:0,000}", ApplyFormatInEditMode = true)]
            [Display(Name = "Tổng giá")]
            public Nullable<decimal> TotalPrice { get; set; }
            [Display(Name = "Trạng thái")]
            public Nullable<int> Status { get; set; }
        }
    }
}