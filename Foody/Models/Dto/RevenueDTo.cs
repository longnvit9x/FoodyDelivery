using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Foody.Models.Dto
{
    public class RevenueDTo
    {
        public int Day { get; set; }
        public decimal Money { get; set; }
        public int NumberInvoice { get; set; }
        public int NumberFood { get; set; }
    }
}