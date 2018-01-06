using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Foody.Models
{
    public partial class InvoiceDetail
    {
        [MetadataTypeAttribute(typeof(InvoiceDetailMetadata))]
        internal sealed class InvoiceDetailMetadata
        {
            [Display(Name = "Mã")]
            public int ID { get; set; }
            [Display(Name = "Món ăn")]
            public Nullable<System.Guid> FoodID { get; set; }
            [Display(Name = "Số lượng")]
            public Nullable<int> NumberFood { get; set; }
            [Display(Name = "Kích thước")]
            public string Size { get; set; }
            [Display(Name = "Giá kích thước")]
            [DisplayFormat(DataFormatString = "{0:0,000}", ApplyFormatInEditMode = true)]
            public Nullable<decimal> PriceSize { get; set; }
            [Display(Name = "Vị")]
            public string Type { get; set; }
            [Display(Name = "Giá vị")]
            [DisplayFormat(DataFormatString = "{0:0,000}", ApplyFormatInEditMode = true)]
            public Nullable<decimal> PriceType { get; set; }
        }
    }
}