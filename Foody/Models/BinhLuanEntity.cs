using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Foody.Models
{
    public class BinhLuanEntity
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public int Rating { get; set; }
        public string StoreID { get; set; }
        public string CustomerID { get; set; }
        public List<FileDataUploadEntity> ListFileUpload { get; set; }
    }
   
}