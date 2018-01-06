using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace Foody.Models
{
    [MetadataTypeAttribute(typeof(PostMetadata))]
    public partial class Post
    {
        internal sealed class PostMetadata
        {
            [Key]
            [Display(Name = "Mã bài viết")]
            public int PostID { get; set; }
            [Display(Name = "Chủ đề")]
            public string Title { get; set; }
            [Display(Name = "Nội dung")]
            public string Content { get; set; }
            [Display(Name = "Đánh giá")]
            public Nullable<int> Rating { get; set; }
            [Display(Name = "Thời gian")]
            public Nullable<System.DateTime> PostTime { get; set; }
            [Display(Name = "Lượt thích")]
            public Nullable<int> Like { get; set; }
            [Display(Name = "Lượt xem")]
            public Nullable<int> View { get; set; }
            [Display(Name = "Cửa hàng")]
            public Nullable<System.Guid> StoreID { get; set; }
            [Display(Name = "Người đăng")]
            public Nullable<System.Guid> CustomerID { get; set; }
        }
    }
}