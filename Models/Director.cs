using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CMS.Models;

[Table("director")]
public partial class Director
{
    [Key]
    [Column("director_id")]
    public int DirectorId { get; set; }

    [Column("director_name")]
    [StringLength(100)]
    [Unicode(false)]
    public string DirectorName { get; set; } = null!;

    [InverseProperty("Director")]
    public virtual ICollection<Movie> Movies { get; set; } = new List<Movie>();
}
