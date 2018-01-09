using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Foody.Models.Entity
{
    public class FoodSizeEntity
    {
        public int FoodSizeID { get; set; }
        public string SizeName { get; set; }
        public Nullable<decimal> PriceSize { get; set; }
        public Nullable<System.Guid> FoodID { get; set; }
    }
}