using System;
using System.Collections.Generic;
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

    public virtual DbSet<MovieCast> MovieCasts { get; set; }

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
        modelBuilder.Entity<BookingSeat>(entity =>
        {
            entity.HasKey(e => new { e.BookingId, e.SeatId }); // ✅ Khóa chính kép

            entity.HasOne(d => d.Booking)
                .WithMany(p => p.BookingSeats)
                .HasForeignKey(d => d.BookingId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_booking_seat_booking");

            entity.HasOne(d => d.Seat)
                .WithMany(p => p.BookingSeats)
                .HasForeignKey(d => d.SeatId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_booking_seat_seat");
        });




        // Gọi hàm partial để mở rộng (nếu có file mở rộng khác)
        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
