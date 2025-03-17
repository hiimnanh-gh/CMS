using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CMS.Models;

[Keyless]
[Table("movie_cast")]
public partial class MovieCast
{
    [Column("movie_id")]
    public int MovieId { get; set; }

    [Column("cast_id")]
    public int CastId { get; set; }

    [ForeignKey("CastId")]
    public virtual CastMember Cast { get; set; } = null!;

    [ForeignKey("MovieId")]
    public virtual Movie Movie { get; set; } = null!;
}
