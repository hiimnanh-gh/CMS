using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CMS.Models
{
    [Table("seat")]
    public partial class Seat
    {
        [Key]
        [Column("seat_id")]
        public int SeatId { get; set; }

        [Column("theatre_id")]
        public int TheatreId { get; set; }

        [Column("seat_type_id")]
        public int SeatTypeId { get; set; }

        [Column("seat_row")]
        [StringLength(1)] // Chỉ 1 ký tự (A, B, C, ...)
        [Unicode(false)]
        public string SeatRow { get; set; } = null!;

        [Column("seat_number")]
        public int SeatNumber { get; set; }

        // Quan hệ tới SeatType
        [ForeignKey("SeatTypeId")]
        [InverseProperty("Seats")]
        public virtual SeatType SeatType { get; set; } = null!;

        // Quan hệ tới Theatre
        [ForeignKey("TheatreId")]
        [InverseProperty("Seats")]
        public virtual Theatre Theatre { get; set; } = null!;

        // Liên kết với bảng booking_seat nếu có
        [InverseProperty("Seat")]
        public virtual ICollection<BookingSeat> BookingSeats { get; set; } = new List<BookingSeat>();

    }
}
