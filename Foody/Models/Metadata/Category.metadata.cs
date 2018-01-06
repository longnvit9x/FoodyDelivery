using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Foody.Models
{
    [MetadataTypeAttribute(typeof(CategoryMetadata))]
    public partial class Category
    {
        internal sealed class CategoryMetadata
        {
            [Key]
            [Display(Name = "Mã Danh mục")]
            public System.Guid CategoryID { get; set; }
            [Display(Name = "Tên danh mục")]
            public string CategoryName { get; set; }
        }
    }
}