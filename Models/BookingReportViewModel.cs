using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CMS.Models;
public class BookingReportViewModel
{
    public int BookingId { get; set; }
    public string UserName { get; set; }
    public string MovieName { get; set; }

    // Rạp chiếu phim
    public int? CinemaId { get; set; }
    public string CinemaLocation { get; set; }
    public List<Cinema> Cinemas { get; set; } // Danh sách rạp

    // Theatre (phòng chiếu)
    public int? TheatreId { get; set; }
    public int TheatreNum { get; set; }
    public List<Theatre> Theatres { get; set; } // Danh sách phòng chiếu

    // Ngày & Giờ chiếu
    public DateTime? ShowingDate { get; set; }
    public TimeSpan? ShowingTime { get; set; }
    public List<DateTime> AvailableDates { get; set; } // Ngày có suất chiếu
    public List<TimeSpan> AvailableTimes { get; set; } // Giờ chiếu theo ngày

    // Thông tin khác
    public List<string> Seats { get; set; }
    public decimal TotalFee { get; set; }
    public DateTime BookingDate { get; set; }

    // Danh sách phim
    public int? MovieId { get; set; }
    public List<Movie> Movies { get; set; }
}

