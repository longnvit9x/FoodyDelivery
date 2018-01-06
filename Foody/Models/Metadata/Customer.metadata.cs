using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
namespace Foody.Models
{
    [MetadataTypeAttribute(typeof(CustomerMetadata))]
    public partial class Customer
    {
        internal sealed class CustomerMetadata
        {
            [Key]
            [Display(Name = "Mã người dùng")]
            public System.Guid CustomerID { get; set; }
            [Required(ErrorMessage = "Trường bắt buộc")]
            [Display(Name = "Họ và tên")]
            public string FullName { get; set; }
            [Required(ErrorMessage = "Trường bắt buộc")]
            [Display(Name = "Email")]
            [RegularExpression(@"^([\w-\.]+)@((\[[0-9]{1,3]\.)|(([\w-]+\.)+))([a-zA-Z{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Please enter valid email.")]
            public string Email { get; set; }
            [Display(Name = "Tài khoản")]
            [Required(ErrorMessage = "Trường bắt buộc")]
            public string UserName { get; set; }
            [Display(Name = "Mật khẩu")]
            [Required(ErrorMessage = "Trường bắt buộc")]
            [DataType(DataType.Password)]
            public string Password { get; set; }
            [Display(Name = "Địa chỉ")]
            public string Address { get; set; }
            [Display(Name = "Số điện thoại")]
            public string Mobile { get; set; }
            [Display(Name = "Avatar")]
            public string Avatar { get; set; }
        }
    }
}