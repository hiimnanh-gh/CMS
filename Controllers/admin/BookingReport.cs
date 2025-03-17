using Microsoft.AspNetCore.Mvc;
using CMS.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System;


namespace CMS.Controllers.Admin
{
    public class BookingReportController : Controller
    {
        private readonly CinemaDbContext _dbc;

        public BookingReportController(CinemaDbContext db)
        {
            _dbc = db;
        }

        // GET: /Admin/BookingReport
        public IActionResult Index()
        {
            var bookingReports = _dbc.Bookings
                .Include(b => b.User)
                .Include(b => b.Time)
                    .ThenInclude(t => t.Theatre)
                        .ThenInclude(th => th.Cinema)
                .Include(b => b.BookingSeats)
                    .ThenInclude(bs => bs.Seat)
                .OrderByDescending(b => b.CreatedDatetime)
                .Select(b => new BookingReportViewModel
                {
                    BookingId = b.BookingId,
                    UserName = b.User.UserName,
                    // Lấy MovieName thông qua movie_theatre
                    MovieName = _dbc.MovieTheatres
                                    .Include(mt => mt.Movie)
                                    .Where(mt => mt.TheatreId == b.Time.TheatreId)
                                    .Select(mt => mt.Movie.Title)
                                    .FirstOrDefault(),
                    CinemaLocation = b.Time.Theatre.Cinema.CinemaAddress,
                    TheatreNum = b.Time.Theatre.TheatreNum,
                    ShowingDate = b.Time.ShowingDate,
                    ShowingTime = b.Time.ShowingDatetime,
                    Seats = b.BookingSeats.Select(bs => bs.Seat.SeatRow + bs.Seat.SeatNumber).ToList(),
                    TotalFee = b.BookingFee,
                    BookingDate = b.CreatedDatetime
                })
                .ToList();

            return View(bookingReports);
        }
    }
}
