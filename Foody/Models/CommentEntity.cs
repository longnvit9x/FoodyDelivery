using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Foody.Models
{
    public class CommentEntity
    {
        public string CommentContent { get; set; }
        public int PostID { get; set; }
        public int CommentLike { get; set; }
        public string CustomerID { get; set; }
        public string CommentTime { get; set; }
    }
}