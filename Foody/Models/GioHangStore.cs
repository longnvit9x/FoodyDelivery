using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Foody.Models
{
    public class GioHangStore
    {
        public string StoreID { get; set; }
        public List<GioHang> Orders { get; set; }
    }
}