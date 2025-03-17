using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CMS.Models
{
    [Table("movie_theatre")]
    public class MovieTheatre
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("movie_id")]
        public int MovieId { get; set; }

        [Column("theatre_id")]
        public int TheatreId { get; set; }

        [ForeignKey(nameof(MovieId))]
        public virtual Movie Movie { get; set; }

        [ForeignKey(nameof(TheatreId))]
        public virtual Theatre Theatre { get; set; }
    }
}
