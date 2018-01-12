using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Foody.Models.Dto
{
    public class DataDateDto
    {
        public string StoreID { get; set; }
        public int Day { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
    }
}