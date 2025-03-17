using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CMS.Models;

[Table("theatre")]
public partial class Theatre
{
    [Key]
    [Column("theatre_id")]
    public int TheatreId { get; set; }

    [Column("cinema_id")]
    public int CinemaId { get; set; }

    [Column("theatre_num")]
    public int TheatreNum { get; set; }

    [Column("available_seats")]
    public int AvailableSeats { get; set; }

    [ForeignKey("CinemaId")]
    [InverseProperty("Theatres")]
    public virtual Cinema Cinema { get; set; } = null!;

    [InverseProperty("Theatre")]
    public virtual ICollection<Seat> Seats { get; set; } = new List<Seat>();

    [InverseProperty("Theatre")]
    public virtual ICollection<ShowingTime> ShowingTimes { get; set; } = new List<ShowingTime>();

    public virtual ICollection<MovieTheatre> MovieTheatres { get; set; } = new List<MovieTheatre>();

}
