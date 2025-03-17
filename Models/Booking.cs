using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CMS.Models;

[Table("booking")]
public partial class Booking
{
    [Key]
    [Column("booking_id")]
    public int BookingId { get; set; }

    [Column("time_id")]
    public int TimeId { get; set; }

    [Column("user_id")]
    public int UserId { get; set; } // Add user_id column

    [Column("booking_fee", TypeName = "decimal(10, 2)")]
    public decimal BookingFee { get; set; }

    [Column("email_address")]
    [StringLength(100)]
    [Unicode(false)]
    public string EmailAddress { get; set; } = null!;

    [Column("created_datetime", TypeName = "datetime")]
    public DateTime CreatedDatetime { get; set; }

    // Foreign key relationships
    [ForeignKey("TimeId")]
    [InverseProperty("Bookings")]
    public virtual ShowingTime Time { get; set; } = null!;

    [ForeignKey("UserId")]
    [InverseProperty("Bookings")]
    public virtual User User { get; set; } = null!; // Add navigation property

    public virtual ICollection<BookingSeat> BookingSeats { get; set; } = new List<BookingSeat>();

}
