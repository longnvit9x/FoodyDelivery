using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Foody.Models
{
    public class LikeEntity
    {
        public int PostID { get; set; }
        public bool isLike { get; set; }
    }
}