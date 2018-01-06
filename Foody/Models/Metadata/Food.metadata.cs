using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Foody.Models
{
    [MetadataTypeAttribute(typeof(FoodMetadata))]
    public partial class Food
    {
        internal sealed class FoodMetadata
        {
            [Display(Name = "Mã món ăn")]
            public System.Guid FoodID { get; set; }
            [Display(Name = "Tên món ăn")]
            [Required(ErrorMessage = "Vui lòng nhập dữ liệu cho trường này.")]
            public string FoodName { get; set; }
            [Display(Name = "Giá")]
            [DisplayFormat(DataFormatString = "{0:0,0}", ApplyFormatInEditMode = true)]
            [Required(ErrorMessage = "Vui lòng nhập dữ liệu cho trường này.")]
            public Nullable<decimal> Price { get; set; }
            [Display(Name = "Ảnh")]
            [Required(ErrorMessage = "Vui lòng nhập dữ liệu cho trường này.")]
            public string FoodImage { get; set; }
            [Display(Name = "Tên Danh mục")]
            [Required(ErrorMessage = "Vui lòng nhập dữ liệu cho trường này.")]
            public Nullable<System.Guid> CategoryID { get; set; }
            [Display(Name = "Cửa hàng")]
            [Required(ErrorMessage = "Vui lòng nhập dữ liệu cho trường này.")]
            public Nullable<System.Guid> StoreID { get; set; }
        }
    }
}