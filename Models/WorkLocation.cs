using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CMS.Models;

[Keyless]
[Table("work_location")]
[Index("UserId", "CinemaId", Name = "unique_user_cinema", IsUnique = true)]
public partial class WorkLocation
{
    [Column("cinema_id")]
    public int CinemaId { get; set; }

    [Column("user_id")]
    public int UserId { get; set; }

    [ForeignKey("CinemaId")]
    public virtual Cinema Cinema { get; set; } = null!;

    [ForeignKey("UserId")]
    public virtual User User { get; set; } = null!;
}
