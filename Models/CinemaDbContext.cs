using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CMS.Models;

public partial class CinemaDbContext : DbContext
{
    public CinemaDbContext()
    {
    }

    public CinemaDbContext(DbContextOptions<CinemaDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Booking> Bookings { get; set; }

    public virtual DbSet<BookingSeat> BookingSeats { get; set; }

    public virtual DbSet<CastMember> CastMembers { get; set; }

    public virtual DbSet<Cinema> Cinemas { get; set; }

    public virtual DbSet<Director> Directors { get; set; }

    public virtual DbSet<Genre> Genres { get; set; }

    public virtual DbSet<Movie> Movies { get; set; }

    public virtual DbSet<MovieGenre> MovieGenres { get; set; }

    public virtual DbSet<MovieStatus> MovieStatuses { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Seat> Seats { get; set; }

    public virtual DbSet<SeatType> SeatTypes { get; set; }

    public virtual DbSet<ShowingTime> ShowingTimes { get; set; }

    public virtual DbSet<Theatre> Theatres { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<WorkLocation> WorkLocations { get; set; }

    public virtual DbSet<MovieTheatre> MovieTheatres { get; set; }



    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Khóa chính phức hợp cho bảng MovieGenre
        modelBuilder.Entity<MovieGenre>()
            .HasKey(mg => new { mg.MovieId, mg.GenreId });

        // Liên kết Movie
        modelBuilder.Entity<MovieGenre>()
            .HasOne(mg => mg.Movie)
            .WithMany(m => m.MovieGenres)
            .HasForeignKey(mg => mg.MovieId);

        // Liên kết Genre
        modelBuilder.Entity<MovieGenre>()
            .HasOne(mg => mg.Genre)
            .WithMany(g => g.MovieGenres)
            .HasForeignKey(mg => mg.GenreId);

        // Khóa chính phức hợp cho bảng MovieTheatre
        modelBuilder.Entity<MovieTheatre>()
            .HasKey(mt => new { mt.MovieId, mt.TheatreId });

        // Liên kết Movie
        modelBuilder.Entity<MovieTheatre>()
            .HasOne(mt => mt.Movie)
            .WithMany(m => m.MovieTheatres)
            .HasForeignKey(mt => mt.MovieId);

        // Liên kết Theatre
        modelBuilder.Entity<MovieTheatre>()
            .HasOne(mt => mt.Theatre)
            .WithMany(t => t.MovieTheatres)
            .HasForeignKey(mt => mt.TheatreId);

        // Khóa chính phức hợp cho bảng BookingSeat
        modelBuilder.Entity<BookingSeat>()
            .HasKey(bs => new { bs.BookingId, bs.SeatId });

        // Liên kết Booking
        modelBuilder.Entity<BookingSeat>()
            .HasOne(bs => bs.Booking)
            .WithMany(b => b.BookingSeats)
            .HasForeignKey(bs => bs.BookingId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_booking_seat_booking");

        // Liên kết Seat
        modelBuilder.Entity<BookingSeat>()
            .HasOne(bs => bs.Seat)
            .WithMany(s => s.BookingSeats)
            .HasForeignKey(bs => bs.SeatId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_booking_seat_seat");

        // Các cấu hình khác có thể giữ nguyên, ví dụ:
        modelBuilder.Entity<BookingSeat>(entity =>
        {
            entity.HasKey(e => new { e.BookingId, e.SeatId });
        });

        // Gọi hàm partial để mở rộng (nếu có file mở rộng khác)
        OnModelCreatingPartial(modelBuilder);
    }


    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
