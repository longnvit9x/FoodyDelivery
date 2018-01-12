using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Foody.Models.Entity;
namespace Foody.Models.Dto
{
    public class ThongKeDto
    {
        public List<InvoiceDto> Invoices { get; set; }
        public List<PostDto> Posts { get; set; }
        public List<RevenueDTo> Revenues { get; set; }
    }
}