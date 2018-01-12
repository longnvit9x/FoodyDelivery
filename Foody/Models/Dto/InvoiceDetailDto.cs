using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Foody.Models.Dto
{
    public class InvoiceDetailDto
    {
        public int ID { get; set; }
        public string FoodID { get; set; }
        public int NumberFood { get; set; }
        public string InvoiceID { get; set; }
        public string FoodImage { get; set; }
        public string FoodName { get; set; }
    }
}