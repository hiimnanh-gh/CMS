using Microsoft.AspNetCore.Mvc;
using CMS.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

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
        public IActionResult Index(int? movieId, int? cinemaId, string sortBy, string sortOrder)
        {
            // Truy vấn danh sách đặt vé
            var bookingsQuery = _dbc.Bookings
                .Include(b => b.User)
                .Include(b => b.Time)
                    .ThenInclude(t => t.Theatre)
                        .ThenInclude(th => th.Cinema)
                .Include(b => b.BookingSeats)
                    .ThenInclude(bs => bs.Seat)
                .AsQueryable();

            // Lọc theo movieId
            if (movieId.HasValue)
            {
                bookingsQuery = bookingsQuery.Where(b =>
                    _dbc.MovieTheatres.Any(mt => mt.MovieId == movieId && mt.TheatreId == b.Time.TheatreId));
            }

            // Lọc theo CinemaId
            if (cinemaId.HasValue)
            {
                bookingsQuery = bookingsQuery.Where(b => b.Time.Theatre.CinemaId == cinemaId);
            }

            // Sắp xếp dữ liệu
            sortBy = string.IsNullOrEmpty(sortBy) ? "BookingDate" : sortBy;
            sortOrder = sortOrder == "asc" ? "asc" : "desc";

            bookingsQuery = sortBy switch
            {
                "MovieName" => (sortOrder == "asc") ? bookingsQuery.OrderBy(b => b.Time.Theatre.MovieTheatres.First().Movie.Title) :
                                                      bookingsQuery.OrderByDescending(b => b.Time.Theatre.MovieTheatres.First().Movie.Title),
                "Cinema" => (sortOrder == "asc") ? bookingsQuery.OrderBy(b => b.Time.Theatre.Cinema.CinemaName) :
                                                   bookingsQuery.OrderByDescending(b => b.Time.Theatre.Cinema.CinemaName),
                "TotalFee" => (sortOrder == "asc") ? bookingsQuery.OrderBy(b => b.BookingFee) :
                                                     bookingsQuery.OrderByDescending(b => b.BookingFee),
                _ => bookingsQuery.OrderByDescending(b => b.CreatedDatetime)
            };

            // Đưa danh sách vào ViewModel
            var bookingReports = bookingsQuery
                .Select(b => new BookingReportViewModel
                {
                    BookingId = b.BookingId,
                    UserName = b.User.UserName,
                    MovieName = b.Time.Theatre.MovieTheatres.FirstOrDefault().Movie.Title,
                    CinemaLocation = b.Time.Theatre.Cinema.CinemaName,
                    TheatreNum = b.Time.Theatre.TheatreNum,
                    ShowingDate = b.Time.ShowingDate,
                    ShowingTime = b.Time.ShowingDatetime,
                    Seats = b.BookingSeats.Select(bs => bs.Seat.SeatRow + bs.Seat.SeatNumber).ToList(),
                    TotalFee = b.BookingFee,
                    BookingDate = b.CreatedDatetime
                }).ToList();

            // Lấy danh sách Movies, Cinemas
            ViewBag.Movies = _dbc.Movies.ToList();
            ViewBag.Cinemas = _dbc.Cinemas.ToList();

            return View(bookingReports);
        }
    }
}
