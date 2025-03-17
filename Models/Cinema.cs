using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CMS.Models;

[Table("cinema")]
public partial class Cinema
{
    [Key]
    [Column("cinema_id")]
    public int CinemaId { get; set; }

    [Column("cinema_name")]
    [StringLength(100)]
    [Unicode(false)]
    public string CinemaName { get; set; } = null!;

    [Column("cinema_address")]
    [StringLength(200)]
    [Unicode(false)]
    public string CinemaAddress { get; set; } = null!;

    [InverseProperty("Cinema")]
    public virtual ICollection<Theatre> Theatres { get; set; } = new List<Theatre>();
}
