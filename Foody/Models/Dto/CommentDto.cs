using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Foody.Models.Dto
{
    public class CommentDto
    {
        public string CommentContent { get; set; }
        public int CommentLike { get; set; }
        public string CommentTime { get; set; }
        public string Avatar { get; set; }
        public string FullName { get; set; }
    }
}