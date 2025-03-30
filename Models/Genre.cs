using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CMS.Models;

[Table("genre")]
public partial class Genre
{
    [Key]
    [Column("genre_id")]
    public int GenreId { get; set; }

    [Column("genre_name")]
    [StringLength(50)]
    [Unicode(false)]
    public string GenreName { get; set; } = null!;
    public virtual ICollection<MovieGenre> MovieGenres { get; set; }
}
