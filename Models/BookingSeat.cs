using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CMS.Models;

[Table("booking_seat")]
[Index("SeatId", "BookingId", Name = "unique_seat_booking", IsUnique = true)]
public partial class BookingSeat
{
    [Key]
    [Column("booking_id")]
    public int BookingId { get; set; }

    [Key]
    [Column("seat_id")]
    public int SeatId { get; set; }

    [ForeignKey("BookingId")]
    [InverseProperty("BookingSeats")]
    public virtual Booking Booking { get; set; } = null!;

    [ForeignKey("SeatId")]
    [InverseProperty("BookingSeats")]
    public virtual Seat Seat { get; set; } = null!;
}
