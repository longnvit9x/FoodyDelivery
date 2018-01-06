using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
//using 2 thư viện thiết kế class metadata
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Foody.Models
{
    [MetadataTypeAttribute(typeof(StoreMetadata))]
    public partial class Store
    {
        internal sealed class StoreMetadata
        {
            [Display(Name = "Mã cửa hàng")]
            public string StoreID { get; set; }
            [Display(Name = "Tên cửa hàng")]
            [Required(ErrorMessage = "Vui lòng nhập dữ liệu cho trường này.")]
            public string StoreName { get; set; }
            [Display(Name = "Địa chỉ cửa hàng")]
            [Required(ErrorMessage = "Vui lòng nhập dữ liệu cho trường này.")]
            public string Address { get; set; }
            [DataType(DataType.Time)]
            [DisplayFormat(DataFormatString = "{0:hh\\:mm}", ApplyFormatInEditMode = true)]
            [Display(Name = "Giờ mở cửa")]
            [Required(ErrorMessage = "Vui lòng nhập dữ liệu cho trường này.")]
            public Nullable<System.TimeSpan> OpenDoor { get; set; }
            [DataType(DataType.Time)]
            [DisplayFormat(DataFormatString = "{0:hh\\:mm}", ApplyFormatInEditMode = true)]
            [Display(Name = "Giờ đóng cửa")]
            [Required(ErrorMessage = "Vui lòng nhập dữ liệu cho trường này.")]
            public Nullable<System.TimeSpan> CloserDoor { get; set; }
            [Display(Name = "Giá dịch vụ")]
            [Required(ErrorMessage = "Vui lòng nhập dữ liệu cho trường này.")]
            [DisplayFormat(DataFormatString = "{0:0,0}", ApplyFormatInEditMode = true)]
            public Nullable<decimal> ServiceCharge { get; set; }
            [Display(Name = "Phí ship")]
            [Required(ErrorMessage = "Vui lòng nhập dữ liệu cho trường này.")]
            [DisplayFormat(DataFormatString = "{0:0,0}", ApplyFormatInEditMode = true)]
            public Nullable<decimal> ShippingFee { get; set; }
            [Display(Name = "Phương thức bán hàng")]
            [Required(ErrorMessage = "Vui lòng nhập dữ liệu cho trường này.")]
            public string Manner { get; set; }
            [Display(Name = "Website")]
            public string Website { get; set; }
            [Display(Name = "Loại cửa hàng")]
            [Required(ErrorMessage = "Vui lòng nhập dữ liệu cho trường này.")]
            public string StoreType { get; set; }
            [Display(Name = "Ảnh hiển thị")]
            [Required(ErrorMessage = "Vui lòng nhập dữ liệu cho trường này.")]
            public string StoreBanner { get; set; }
            [Display(Name = "Mở cửa từ thứ")]
            public Nullable<int> StartDate { get; set; }
            [Display(Name = "Đến thứ")]
            public Nullable<int> EndDate { get; set; }
        }
    }
}