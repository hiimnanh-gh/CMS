using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CMS.Models;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CMS.Controllers.Staff

{
    public class BookingManagementController : Controller
    {
        private readonly CinemaDbContext _dbc;

        public BookingManagementController(CinemaDbContext dbc)
        {
            _dbc = dbc;
        }

        public IActionResult Index()
        {
            var bookings = _dbc.Bookings
                .Include(b => b.User)
                .Include(b => b.Time).ThenInclude(st => st.Theatre)
                .Include(b => b.BookingSeats).ThenInclude(bs => bs.Seat)
                .Select(b => new BookingIndexViewModel
                {
                    BookingId = b.BookingId,
                    CustomerName = b.User.UserName,
                    TheatreLocation = b.Time.Theatre.Cinema.CinemaAddress,
                    TheatreNum = b.Time.Theatre.TheatreNum,
                    ShowingDateTime = b.Time.ShowingDatetime,
                    Seats = b.BookingSeats.Select(bs => bs.Seat.SeatRow + bs.Seat.SeatNumber).ToList(),
                    TotalFee = b.BookingFee,
                    CreatedDate = b.CreatedDatetime
                }).ToList();
            
            return View(bookings);
        }





        // ========================= CHI TIẾT ĐƠN ĐẶT VÉ =========================
        public IActionResult Details(int id)
        {
            var booking = _dbc.Bookings
                .Include(b => b.Time)
                    .ThenInclude(t => t.Theatre)
                        .ThenInclude(th => th.Cinema)
                .Include(b => b.User)
                .Include(b => b.BookingSeats)
                    .ThenInclude(bs => bs.Seat)
                .FirstOrDefault(b => b.BookingId == id);


            if (booking == null)
            {
                return NotFound();
            }

            // Lấy phim qua bảng trung gian movie_theatre hoặc trực tiếp nếu có
            var movie = _dbc.MovieTheatres
                .Include(mt => mt.Movie)
                .FirstOrDefault(mt => mt.TheatreId == booking.Time.TheatreId)?.Movie;

            ViewBag.MovieName = movie?.Title ?? "Chưa xác định";

            return View(booking);
        }


        // ========================= THÊM ĐƠN ĐẶT VÉ =========================
        public IActionResult Create()
        {
            ViewBag.ShowingTimes = _dbc.ShowingTimes.Include(st => st.Theatre).ToList();
            ViewBag.Users = _dbc.Users.ToList();
            ViewBag.Seats = _dbc.Seats.ToList();
            return View();
        }

        [HttpPost]
        public IActionResult Create(Booking booking, List<int> selectedSeats)
        {
            if (ModelState.IsValid)
            {
                booking.CreatedDatetime = DateTime.Now;
                _dbc.Bookings.Add(booking);
                _dbc.SaveChanges();

                // Lưu danh sách ghế
                foreach (var seatId in selectedSeats)
                {
                    _dbc.BookingSeats.Add(new BookingSeat
                    {
                        BookingId = booking.BookingId,
                        SeatId = seatId
                    });
                }
                _dbc.SaveChanges();
                return RedirectToAction("Index");
            }

            // Nếu lỗi thì load lại dữ liệu để hiển thị form
            ViewBag.ShowingTimes = _dbc.ShowingTimes.Include(st => st.Theatre).ToList();
            ViewBag.Users = _dbc.Users.ToList();
            ViewBag.Seats = _dbc.Seats.ToList();
            return View(booking);
        }

        // ========================= SỬA ĐƠN ĐẶT VÉ =========================
        public IActionResult Edit(int id)
        {
            var booking = _dbc.Bookings
                .Include(b => b.User)
                .Include(b => b.Time)
                    .ThenInclude(t => t.Theatre)
                        .ThenInclude(th => th.Cinema)
                .Include(b => b.BookingSeats)
                .FirstOrDefault(b => b.BookingId == id);

            if (booking == null)
            {
                return NotFound();
            }

            // Lấy danh sách phòng chiếu (theatre)
            ViewBag.TheatreList = new SelectList(_dbc.Theatres.Include(t => t.Cinema).ToList(), "TheatreId", "Cinema.CinemaAddress");

            // Lấy danh sách ghế
            ViewBag.SeatList = _dbc.Seats.ToList();

            return View(booking);
        }

        [HttpPost]
        public IActionResult Edit(int bookingId, int userId, DateTime createdDatetime, int SelectedTheatreId, DateTime SelectedDate, TimeSpan SelectedTime, List<int> SelectedSeatIds)
        {
            var booking = _dbc.Bookings
                .Include(b => b.BookingSeats)
                .FirstOrDefault(b => b.BookingId == bookingId);

            if (booking == null)
            {
                return NotFound();
            }

            // Cập nhật các thông tin
            booking.UserId = userId;
            booking.CreatedDatetime = createdDatetime;

            // Kiểm tra suất chiếu đã tồn tại chưa
            var showingTime = _dbc.ShowingTimes.FirstOrDefault(s =>
                s.TheatreId == SelectedTheatreId &&
                s.ShowingDate == SelectedDate &&
                s.ShowingDatetime == SelectedTime);

            // Nếu chưa có suất chiếu thì tạo mới
            if (showingTime == null)
            {
                showingTime = new ShowingTime
                {
                    TheatreId = SelectedTheatreId,
                    ShowingDate = SelectedDate,
                    ShowingDatetime = SelectedTime
                };
                _dbc.ShowingTimes.Add(showingTime);
                _dbc.SaveChanges(); // Lưu để lấy TimeId
            }

            // Gán suất chiếu mới
            booking.TimeId = showingTime.TimeId;

            // Cập nhật lại danh sách ghế
            booking.BookingSeats.Clear();
            foreach (var seatId in SelectedSeatIds)
            {
                booking.BookingSeats.Add(new BookingSeat
                {
                    BookingId = booking.BookingId,
                    SeatId = seatId
                });
            }

            // Tính lại tổng tiền (ví dụ mỗi ghế 100.000 VNĐ)
            decimal pricePerSeat = 100000;
            booking.BookingFee = pricePerSeat * SelectedSeatIds.Count;

            _dbc.SaveChanges();

            return RedirectToAction("Index");
        }











        // ========================= XÓA ĐƠN ĐẶT VÉ =========================
        // GET: BookingManagement/Delete/20
        public IActionResult Delete(int id)
        {
            var booking = _dbc.Bookings
                .Include(b => b.BookingSeats)
                .FirstOrDefault(b => b.BookingId == id);

            if (booking == null) return NotFound();

            return View(booking);
        }

        // POST: BookingManagement/DeleteConfirmed
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var booking = _dbc.Bookings.FirstOrDefault(b => b.BookingId == id);

            if (booking != null)
            {
                // Xóa tất cả các ghế đã đặt liên quan đến đơn đặt vé
                _dbc.Database.ExecuteSqlRaw("DELETE FROM booking_seat WHERE booking_id = {0}", id);

                // Xóa đơn đặt vé
                _dbc.Bookings.Remove(booking);
                _dbc.SaveChanges();
            }

            return RedirectToAction("Index");
        }






    }
}
