using Microsoft.AspNetCore.Mvc;
using CMS.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System;

namespace CMS.Controllers
{
    public class OrderController : Controller
    {
        private readonly CinemaDbContext _dbc;

        public OrderController(CinemaDbContext db)
        {
            _dbc = db;
        }

        // GET: /Order/Index
        public IActionResult Index()
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login", "Home"); // Nếu chưa có UserId trong session, chuyển về trang chủ
            }

            var customer = _dbc.Users.FirstOrDefault(u => u.UserId == userId);
            if (customer == null)
            {
                return NotFound();
            }

            var myOrders = _dbc.Bookings
                .Include(b => b.User)
                .Include(b => b.Time)
                    .ThenInclude(t => t.Theatre)
                        .ThenInclude(th => th.Cinema)
                .Include(b => b.BookingSeats)
                    .ThenInclude(bs => bs.Seat)
                .Where(b => b.UserId == userId)
                .OrderByDescending(b => b.CreatedDatetime)
                .Select(b => new BookingReportViewModel
                {
                    BookingId = b.BookingId,
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

            return View(myOrders);
        }

    }
}
