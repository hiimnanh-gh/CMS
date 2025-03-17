using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;

namespace CMS.Models;

[Table("users")]
[Index("Email", Name = "UQ__users__AB6E6164704975E0", IsUnique = true)]
public partial class User
{
    [Key]
    [Column("user_id")]
    public int UserId { get; set; }

    [Column("user_name")]
    [StringLength(100)]
    [Unicode(false)]
    [Required(ErrorMessage = "Tên là bắt buộc")]
    public string UserName { get; set; } = null!;

    [Column("role_id")]
    [Required(ErrorMessage = "Vai trò là bắt buộc")]
    public int RoleId { get; set; }

    [Column("email")]
    [StringLength(100)]
    [Unicode(false)]
    [Required(ErrorMessage = "Email là bắt buộc")]
    [EmailAddress(ErrorMessage = "Email không hợp lệ")]
    public string Email { get; set; } = null!;

    [Column("phone_num")]
    [StringLength(20)]
    [Unicode(false)]
    [Required(ErrorMessage = "Số điện thoại là bắt buộc")]
    public string? PhoneNum { get; set; } = null!;

    [BindNever] // Không bind PasswordHash từ form
    [Column("password_hash")]
    [StringLength(255)]
    [Unicode(false)]
    [Required(ErrorMessage = "Mật khẩu là bắt buộc")]
    public string PasswordHash { get; set; } = null!;

    // Khóa ngoại đến bảng Role
    [ForeignKey("RoleId")]
    [InverseProperty("Users")]
    public virtual Role Role { get; set; } = null!;

    // Khóa ngoại đến bảng Booking
    [InverseProperty("User")]
    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    // Phương thức hash mật khẩu
    public void SetPassword(string password)
    {
        PasswordHash = BCrypt.Net.BCrypt.HashPassword(password);
    }

    // Kiểm tra mật khẩu
    public bool VerifyPassword(string password)
    {
        return BCrypt.Net.BCrypt.Verify(password, PasswordHash);
    }
}
