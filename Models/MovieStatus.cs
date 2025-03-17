using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CMS.Models;

[Table("movie_status")]
[Index("StatusName", Name = "UQ__movie_st__501B37536219D8F3", IsUnique = true)]
public partial class MovieStatus
{
    [Key]
    [Column("status_id")]
    public int StatusId { get; set; }

    [Column("status_name")]
    [StringLength(50)]
    [Unicode(false)]
    public string StatusName { get; set; } = null!;

    [InverseProperty("Status")]
    public virtual ICollection<Movie> Movies { get; set; } = new List<Movie>();
}
