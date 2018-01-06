using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Foody.Models
{
    [MetadataTypeAttribute(typeof(FoodTypeMetadata))]
    public partial class FoodType
    {
        internal sealed class FoodTypeMetadata
        {
            [Display(Name = "Mã loại")]
            public int FoodTypeID { get; set; }
            [Display(Name = "Tên loại")]
            [Required(ErrorMessage = "Vui lòng nhập dữ liệu cho trường này.")]
            public string TypeName { get; set; }
            [Display(Name = "Giá loại")]
            [DisplayFormat(DataFormatString = "{0:0,0}", ApplyFormatInEditMode = true)]
            [Required(ErrorMessage = "Vui lòng nhập dữ liệu cho trường này.")]
            public Nullable<decimal> PriceType { get; set; }
            [Display(Name = "Tên món ăn")]
            [Required(ErrorMessage = "Vui lòng nhập dữ liệu cho trường này.")]
            public Nullable<System.Guid> FoodID { get; set; }
        }
    }
}