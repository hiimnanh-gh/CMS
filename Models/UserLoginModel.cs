using System.ComponentModel.DataAnnotations;

namespace CMS.Models
{
    public class UserLoginModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;

        [Required]
        public string Password { get; set; } = null!;
    }
}
