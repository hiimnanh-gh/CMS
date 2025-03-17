using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace CMS.Models
{
    public class StaffCreateModel
    {
        [Required(ErrorMessage = "Tên nhân viên là bắt buộc")]
        public string UserName { get; set; } = null!;

        [Required(ErrorMessage = "Email là bắt buộc")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Số điện thoại là bắt buộc")]
        public string PhoneNum { get; set; } = null!;

        [Required(ErrorMessage = "Mật khẩu là bắt buộc")]
        public string Password { get; set; } = null!;
    }

}
