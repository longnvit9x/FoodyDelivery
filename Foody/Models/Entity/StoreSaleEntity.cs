using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Foody.Models.Entity
{
    public class StoreSaleEntity
    {
        public int StoreSaleID { get; set; }
        public int Sale { get; set; }
        public string StratSale { get; set; }
        public string StopSale { get; set; }
        public string StoreID { get; set; }
    }
}
