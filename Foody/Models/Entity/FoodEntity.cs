using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Foody.Models.Entity
{
   public class FoodEntity
    {
        public Nullable<System.Guid> FoodID { get; set; }
        public string FoodName { get; set; }
        public Nullable<decimal> Price { get; set; }
        public string FoodImage { get; set; }
        public string FoodImageURL { get; set; }
        public string CategoryID { get; set; }
        public string StoreID { get; set; }
        public Nullable<int> FileID { get; set; }
        public List<FoodSizeEntity> FoodSizes { get; set; }
        public List<FoodTypeEntity> FoodTypes { get; set; }
    }
}
