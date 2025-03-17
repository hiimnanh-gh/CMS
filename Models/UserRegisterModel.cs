using System.ComponentModel.DataAnnotations;

namespace CMS.Models
{
    public class UserRegisterModel
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [Phone]
        [StringLength(20)]
        public string PhoneNum { get; set; } = null!;

        [Required]
        [MinLength(6)]
        public string Password { get; set; }

        [Required]
        [Compare("Password", ErrorMessage = "Mật khẩu nhập lại không khớp!")]
        public string ConfirmPassword { get; set; }
    }
}
