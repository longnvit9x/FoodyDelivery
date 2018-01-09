using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Foody.Models.Dto
{
    public class GioHangDto
    {
        public string StoreID { get; set; }
        public string StoreName { get; set; }
        public int SoPhan { get; set; }
        public string StoreImage { get; set; }
    }
}