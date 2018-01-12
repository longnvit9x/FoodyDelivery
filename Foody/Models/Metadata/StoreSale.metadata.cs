using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Foody.Models
{
    [MetadataTypeAttribute(typeof(StoreSaleMetadata))]
    public partial class StoreSale
    {
        internal sealed class StoreSaleMetadata
        {
            [Display(Name = "Mã sale")]
            public int StoreSaleID { get; set; }
            [Display(Name = "Phần trăm sale")]
            [Required(ErrorMessage = "Vui lòng nhập dữ liệu cho trường này.")]
            public Nullable<int> Sale { get; set; }
            [DataType(DataType.DateTime)]
            [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
            [Display(Name = "Ngày bắt đầu sale")]
            public Nullable<System.DateTime> StartSale { get; set; }
            [DataType(DataType.DateTime)]
            [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
            [Display(Name = "Ngày kết thúc sale")]
            public Nullable<System.DateTime> StopSale { get; set; }
            [Display(Name = "Cửa hàng")]
            [Required(ErrorMessage = "Vui lòng nhập dữ liệu cho trường này.")]
            public Nullable<System.Guid> StoreID { get; set; }
            [Required(ErrorMessage = "Vui lòng nhập dữ liệu cho trường này.")]
            public string KeySale { get; set; }
        }
    }
}