using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CMS.Models;

[Table("movies")]
public partial class Movie
{
    [Key]
    [Column("movie_id")]
    public int MovieId { get; set; }

    [Column("director_id")]
    [Required(ErrorMessage = "The Director field is required.")]
    public int DirectorId { get; set; }

    [Column("status_id")]
    [Required(ErrorMessage = "The Status field is required.")]
    public int StatusId { get; set; }

    [Column("title")]
    [StringLength(200)]
    [Unicode(false)]
    [Required(ErrorMessage = "The Title field is required.")]
    public string Title { get; set; } = null!;

    [Column("age_rating")]
    [StringLength(10)]
    [Unicode(false)]
    public string? AgeRating { get; set; }

    [Column("runtime_min")]
    public int? RuntimeMin { get; set; }

    [Column("release_date")]
    public DateOnly? ReleaseDate { get; set; }

    [Column("trailer_link")]
    [StringLength(255)]
    [Unicode(false)]
    public string? TrailerLink { get; set; }

    [Column("description", TypeName = "text")]
    public string? Description { get; set; }

    [Column("banner_text")]
    [StringLength(255)]
    [Unicode(false)]
    public string? BannerText { get; set; }

    [Column("poster_image")]
    public string? PosterImage { get; set; } // Lưu đường dẫn ảnh

    [Column("synopsis", TypeName = "text")]
    public string? Synopsis { get; set; }

    // ✅ Fix lỗi `NullReferenceException` khi không có dữ liệu Director hoặc Status
    [ForeignKey("DirectorId")]
    [InverseProperty("Movies")]
    public virtual Director? Director { get; set; }

    [ForeignKey("StatusId")]
    [InverseProperty("Movies")]
    public virtual MovieStatus? Status { get; set; }

    // ✅ Thuộc tính không ánh xạ, dùng để nhận tên đạo diễn từ form
    [NotMapped]
    public string? DirectorName { get; set; }

    [NotMapped] // Không lưu vào DB
    public IFormFile? PosterFile { get; set; }


    // Liên kết với MovieGenre
    public virtual ICollection<MovieGenre> MovieGenres { get; set; } = new List<MovieGenre>();

    // Liên kết với MovieTheatre
    public virtual ICollection<MovieTheatre> MovieTheatres { get; set; } = new List<MovieTheatre>();


}
