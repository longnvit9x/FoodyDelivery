using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Foody.Models
{
    public class FileDataUploadEntity
    {
        public string fileName { get; set; }
        public int fileSize { get; set; }
        public string fileType { get; set; }
        public string dataImage { get; set; }
    }
}