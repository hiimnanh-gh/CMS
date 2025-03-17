using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CMS.Models;

[Keyless]
[Table("movie_genre")]
public partial class MovieGenre
{
    [Column("movie_id")]
    public int MovieId { get; set; }

    [Column("genre_id")]
    public int GenreId { get; set; }

    [ForeignKey("GenreId")]
    public virtual Genre Genre { get; set; } = null!;

    [ForeignKey("MovieId")]
    public virtual Movie Movie { get; set; } = null!;
}
