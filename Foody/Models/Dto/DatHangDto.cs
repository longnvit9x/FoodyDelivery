using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Foody.Models.Dto
{
    public class DatHangDto
    {
        public string StoreID { get; set; }
        public int StoreSaleID { get; set; }
        public string CustomerName { get; set; }
        public string CustomerPhone { get; set; }
        public string CustomerAddress { get; set; }
        public string DateDelivery { get; set; }
        public string TimeDelivery { get; set; }
        public int Distance { get; set; }
    }
}