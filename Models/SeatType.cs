using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CMS.Models;

[Table("seat_type")]
public partial class SeatType
{
    [Key]
    [Column("seat_type_id")]
    public int SeatTypeId { get; set; }

    [Column("type_name")]
    [StringLength(50)]
    [Unicode(false)]
    public string TypeName { get; set; } = null!;

    [InverseProperty("SeatType")]
    public virtual ICollection<Seat> Seats { get; set; } = new List<Seat>();
}
