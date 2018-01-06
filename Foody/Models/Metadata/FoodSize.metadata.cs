using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Foody.Models
{
    [MetadataTypeAttribute(typeof(FoodSizeMetadata))]
    public partial class FoodSize
    {
        internal sealed class FoodSizeMetadata
        {
            [Display(Name = "Mã size")]
            public int FoodSizeID { get; set; }
            [Display(Name = "Tên size")]
            [Required(ErrorMessage = "Vui lòng nhập dữ liệu cho trường này.")]
            public string SizeName { get; set; }
            [Display(Name = "Giá size")]
            [DisplayFormat(DataFormatString = "{0:0,0}", ApplyFormatInEditMode = true)]
            [Required(ErrorMessage = "Vui lòng nhập dữ liệu cho trường này.")]
            public Nullable<decimal> PriceSize { get; set; }
            [Display(Name = "Tên món ăn")]
            [Required(ErrorMessage = "Vui lòng nhập dữ liệu cho trường này.")]
            public Nullable<System.Guid> FoodID { get; set; }
        }
    }
}