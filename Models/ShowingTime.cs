using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CMS.Models;

[Table("showing_time")]
public partial class ShowingTime
{
    [Key]
    [Column("time_id")]
    public int TimeId { get; set; }

    [Column("theatre_id")]
    public int TheatreId { get; set; }

    // Giờ chiếu mặc định (chỉ lưu giờ, không lưu ngày)
    [Column("showing_datetime", TypeName = "time")]
    public TimeSpan ShowingDatetime { get; set; }

    // Thêm ngày chiếu (cho phép chọn nhiều ngày cho 1 giờ chiếu)
    [Column("showing_date", TypeName = "date")]
    [DataType(DataType.Date)]
    public DateTime? ShowingDate { get; set; } // PHẢI có dấu hỏi (nullable)


    // Liên kết 1-n với bảng Booking (suất chiếu được đặt vé)
    [InverseProperty("Time")]
    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    // Liên kết 1-1 với bảng Theatre (Phòng chiếu)
    [ForeignKey("TheatreId")]
    [InverseProperty("ShowingTimes")]
    public virtual Theatre Theatre { get; set; } = null!;
}
