using CMS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CMS.Controllers.book
{
    public class BookingController : Controller
    {
        private readonly CinemaDbContext _dbc;

        public BookingController(CinemaDbContext context)
        {
            _dbc = context;
        }

        // B1: Hiển thị thành phố theo phim (city -> cinema)
        public IActionResult SelectCity(int movieId)
        {
            var cityList = _dbc.MovieTheatres
                .Include(mt => mt.Theatre)
                .ThenInclude(t => t.Cinema)
                .Where(mt => mt.MovieId == movieId)
                .Select(mt => new CinemaViewModel
                {
                    CinemaId = mt.Theatre.CinemaId,
                    CinemaName = mt.Theatre.Cinema.CinemaName
                })
                .Distinct()
                .ToList();

            ViewBag.MovieId = movieId;
            return View(cityList);
        }

        // B2: Hiển thị ngày chiếu theo phim + thành phố
        public IActionResult SelectDate(int movieId, int cinemaId)
        {
            var dateList = _dbc.ShowingTimes
                .Include(st => st.Theatre)
                .ThenInclude(t => t.MovieTheatres)
                .Where(st => st.Theatre.CinemaId == cinemaId && st.Theatre.MovieTheatres.Any(mt => mt.MovieId == movieId))
                .Select(st => st.ShowingDate)
                .Distinct()
                .OrderBy(d => d)
                .ToList();

            ViewBag.MovieId = movieId;
            ViewBag.CinemaId = cinemaId;
            return View(dateList);
        }

        // B3: Hiển thị giờ chiếu theo ngày, phim, thành phố
        public IActionResult SelectShowTime(int movieId, int cinemaId, DateTime showingDate)
        {
            var showTimes = _dbc.ShowingTimes
                .Include(st => st.Theatre)
                .ThenInclude(t => t.MovieTheatres)
                .Where(st => st.ShowingDate == showingDate
                             && st.Theatre.CinemaId == cinemaId
                             && st.Theatre.MovieTheatres.Any(mt => mt.MovieId == movieId))
                .Select(st => new ShowTimeViewModel
                {
                    TimeId = st.TimeId,
                    ShowingDatetime = st.ShowingDatetime
                })
                .OrderBy(st => st.ShowingDatetime)
                .ToList();

            ViewBag.MovieId = movieId;
            ViewBag.CinemaId = cinemaId;
            ViewBag.ShowingDate = showingDate.ToString("yyyy-MM-dd");

            return View(showTimes);
        }

        // B4: Chọn ghế theo suất chiếu
        public IActionResult SelectSeats(int timeId)
        {
            var showingTime = _dbc.ShowingTimes
                .Include(st => st.Theatre)
                .FirstOrDefault(st => st.TimeId == timeId);

            if (showingTime == null) return NotFound();

            // Tất cả ghế trong rạp
            var allSeats = _dbc.Seats
                .Where(s => s.TheatreId == showingTime.TheatreId)
                .ToList();

            // Các ghế đã được đặt
            var bookedSeats = _dbc.BookingSeats
                .Where(bs => bs.Booking.TimeId == timeId)
                .Select(bs => bs.SeatId)
                .ToList();

            // Chuẩn bị ViewModel
            var seatViewModels = allSeats.Select(s => new SeatViewModel
            {
                SeatId = s.SeatId,
                SeatRow = s.SeatRow,
                SeatNumber = s.SeatNumber,
                IsBooked = bookedSeats.Contains(s.SeatId) // true nếu đã đặt
            }).ToList();

            ViewBag.TimeId = timeId;
            return View(seatViewModels);


            Console.WriteLine("Số lượng ghế: " + seatViewModels.Count);
            foreach (var seat in seatViewModels)
            {
                Console.WriteLine($"SeatId: {seat.SeatId}, Row: {seat.SeatRow}, Number: {seat.SeatNumber}, Booked: {seat.IsBooked}");
            }

        }



        // B5: Xác nhận đặt vé
        [HttpPost]
        public IActionResult ConfirmBooking(List<int> selectedSeatIds, int timeId)
        {
            var userName = HttpContext.Session.GetString("UserName") ?? "Khách chưa đăng nhập";

            var time = _dbc.ShowingTimes
                .Include(st => st.Theatre)
                .FirstOrDefault(st => st.TimeId == timeId);

            if (time == null) return NotFound();

            var seats = _dbc.Seats
                .Where(s => selectedSeatIds.Contains(s.SeatId))
                .ToList();

            var confirmViewModel = new BookingConfirmViewModel
            {
                UserName = userName, // Lấy từ session
                TheatreName = $"Phòng số {time.Theatre.TheatreNum}",
                ShowingTime = $"{time.ShowingDate?.ToString("dd/MM/yyyy")} - {time.ShowingDatetime}",
                SeatNumbers = seats.Select(s => $"{s.SeatRow}{s.SeatNumber}").ToList(),
                SeatIds = selectedSeatIds,
                TimeId = timeId,
                TotalFee = seats.Count * 100000 // Ví dụ giá 100k/ghế, phần này bạn có thể để nguyên hoặc chỉnh
            };

            return View(confirmViewModel);
        }



        // B6: Lưu booking
    [HttpPost]
    public IActionResult SaveBooking(List<int> seatIds, int timeId)
    {
        // Lấy thông tin user từ Session
        var userId = HttpContext.Session.GetInt32("UserId");
        var userName = HttpContext.Session.GetString("UserName");
        var email = HttpContext.Session.GetString("UserEmail");

        if (userId == null || string.IsNullOrEmpty(userName))
        {
            return RedirectToAction("Login", "Account");
        }

        // Tính phí
        var totalFee = seatIds.Count * 100000; // mỗi ghế 100k

        // Lưu vào bảng Booking
        var booking = new Booking
        {
            UserId = userId.Value,
            TimeId = timeId,
            CreatedDatetime = DateTime.Now,
            EmailAddress = email,
            BookingFee = totalFee
        };
        _dbc.Bookings.Add(booking);
        _dbc.SaveChanges();

        // Lưu các ghế vào bảng BookingSeat
        foreach (var seatId in seatIds)
        {
            var bookingSeat = new BookingSeat
            {
                BookingId = booking.BookingId,
                SeatId = seatId
            };
            _dbc.BookingSeats.Add(bookingSeat);
        }
        _dbc.SaveChanges();

        // Lấy thông tin chi tiết suất chiếu, rạp, phim, phòng
        var showingTime = _dbc.ShowingTimes
            .Include(st => st.Theatre)
                .ThenInclude(t => t.Cinema)
            .FirstOrDefault(st => st.TimeId == timeId);

        if (showingTime == null) return NotFound();

        // Lấy Movie từ bảng movie_theatre theo theatre_id của suất chiếu
        var movie = _dbc.MovieTheatres
            .Include(mt => mt.Movie)
            .FirstOrDefault(mt => mt.TheatreId == showingTime.TheatreId)?.Movie;

        if (movie == null) return NotFound(); // Không tìm thấy phim

        // Danh sách ghế
        var seatList = _dbc.Seats
            .Where(s => seatIds.Contains(s.SeatId))
            .Select(s => s.SeatRow + s.SeatNumber)
            .ToList();

        // Chuẩn bị model cho trang BookingSuccess
        var bookingInfo = new BookingSuccessViewModel
        {
            BookingId = booking.BookingId,
            UserName = userName,
            Seats = seatList,
            TotalFee = totalFee,
            MovieName = movie.Title, // Tên phim lấy qua movie_theatre
            CinemaLocation = showingTime.Theatre.Cinema.CinemaName,
            TheatreNum = showingTime.Theatre.TheatreNum,
            ShowingTime = $"{showingTime.ShowingDate?.ToString("dd/MM/yyyy")} - {showingTime.ShowingDatetime}",
        };


        // Trả về View BookingSuccess
        return View("BookingSuccess", bookingInfo);
    }
       





    }
}
