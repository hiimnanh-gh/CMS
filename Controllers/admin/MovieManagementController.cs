using Microsoft.AspNetCore.Mvc;
using CMS.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System;

namespace CMS.Controllers.Admin
{
    public class MovieManagementController : Controller
    {
        private readonly CinemaDbContext _dbc;

        public MovieManagementController(CinemaDbContext db)
        {
            _dbc = db;
        }

        // ========================= QUẢN LÝ PHIM =========================
        public IActionResult Index(string search)
        {
            var query = _dbc.Movies.Include(m => m.Director).Include(m => m.Status).AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(m => m.Title.Contains(search));
                ViewData["SearchQuery"] = search;
            }

            return View(query.ToList());
        }

        public IActionResult MovieDetails(int id)
        {
            var movie = _dbc.Movies.Include(m => m.Director).Include(m => m.Status).FirstOrDefault(m => m.MovieId == id);
            return movie == null ? NotFound() : View(movie);
        }

        [HttpGet]
        public IActionResult SelectDirector()
        {
            ViewBag.Directors = _dbc.Directors.ToList();
            return View();
        }

        [HttpPost]
        public IActionResult SelectDirector(int? DirectorId, string DirectorName)
        {
            if (DirectorId.HasValue)
                return RedirectToAction("Create", new { directorId = DirectorId.Value });

            if (!string.IsNullOrEmpty(DirectorName))
            {
                var director = new Director { DirectorName = DirectorName };
                _dbc.Directors.Add(director);
                _dbc.SaveChanges();
                return RedirectToAction("Create", new { directorId = director.DirectorId });
            }

            ModelState.AddModelError("", "Vui lòng chọn hoặc thêm đạo diễn.");
            ViewBag.Directors = _dbc.Directors.ToList();
            return View();
        }

        [HttpGet]
        public IActionResult Create(int directorId)
        {
            ViewBag.Director = _dbc.Directors.Find(directorId);
            return View();
        }

        [HttpPost]
        public IActionResult Create(Movie model, int directorId)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Director = _dbc.Directors.Find(directorId);
                return View(model);
            }

            model.DirectorId = directorId;
            model.StatusId = _dbc.MovieStatuses.FirstOrDefault(s => s.StatusName == "Upcoming")?.StatusId ?? 1;

            _dbc.Movies.Add(model);
            _dbc.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var movie = _dbc.Movies.Find(id);
            if (movie == null) return NotFound();

            ViewBag.Directors = _dbc.Directors.ToList();
            ViewBag.Statuses = _dbc.MovieStatuses.ToList();
            return View(movie);
        }

        [HttpPost]
        public IActionResult Edit(int id, Movie movie)
        {
            var existingMovie = _dbc.Movies.Find(id);
            if (existingMovie == null) return NotFound();

            _dbc.Entry(existingMovie).CurrentValues.SetValues(movie);
            _dbc.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var movie = _dbc.Movies.Include(m => m.Director).Include(m => m.Status).FirstOrDefault(m => m.MovieId == id);
            return movie == null ? NotFound() : View(movie);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var movie = _dbc.Movies.Find(id);
            if (movie != null)
            {
                _dbc.Movies.Remove(movie);
                _dbc.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        // ========================= QUẢN LÝ PHÒNG CHIẾU =========================

        public IActionResult ManageTheatres(int movieId)
        {
            var movie = _dbc.Movies.Find(movieId);
            if (movie == null) return NotFound();

            var movieTheatres = _dbc.MovieTheatres.Include(mt => mt.Theatre).Where(mt => mt.MovieId == movieId).ToList();
            ViewBag.Movie = movie;
            return View(movieTheatres);
        }

        [HttpGet]
        public IActionResult AddTheatreToMovie(int movieId)
        {
            var assignedTheatres = _dbc.MovieTheatres.Where(mt => mt.MovieId == movieId).Select(mt => mt.TheatreId);
            var availableTheatres = _dbc.Theatres.Where(t => !assignedTheatres.Contains(t.TheatreId)).ToList();
            ViewBag.Movie = _dbc.Movies.Find(movieId);
            return View(availableTheatres);
        }

        [HttpPost]
        public IActionResult AddTheatreToMovie(int movieId, int theatreId)
        {
            if (!_dbc.MovieTheatres.Any(mt => mt.MovieId == movieId && mt.TheatreId == theatreId))
            {
                _dbc.MovieTheatres.Add(new MovieTheatre { MovieId = movieId, TheatreId = theatreId });
                _dbc.SaveChanges();
            }
            return RedirectToAction("ManageTheatres", new { movieId });
        }

        public IActionResult RemoveTheatre(int movieId, int theatreId)
        {
            var movieTheatre = _dbc.MovieTheatres.FirstOrDefault(mt => mt.MovieId == movieId && mt.TheatreId == theatreId);
            if (movieTheatre != null)
            {
                _dbc.MovieTheatres.Remove(movieTheatre);
                _dbc.SaveChanges();
            }
            return RedirectToAction("ManageTheatres", new { movieId });
        }

        // ========================= QUẢN LÝ NGÀY CHIẾU =========================

        public IActionResult ManageShowingDates(int movieId, int theatreId)
        {
            var showingDates = _dbc.ShowingTimes.Where(st => st.TheatreId == theatreId).Select(st => st.ShowingDate).Distinct().ToList();
            ViewBag.MovieId = movieId;
            ViewBag.TheatreId = theatreId;
            return View(showingDates);
        }

        [HttpGet]
        public IActionResult AddShowingDate(int movieId, int theatreId)
        {
            var movie = _dbc.Movies.Find(movieId);
            var theatre = _dbc.Theatres.Find(theatreId);

            if (movie == null || theatre == null) return NotFound(); // Kiểm tra null tránh lỗi tiếp

            ViewBag.Movie = movie;
            ViewBag.Theatre = theatre;
            ViewBag.MovieId = movieId;
            ViewBag.TheatreId = theatreId;
            return View();
        }

        [HttpPost]
        public IActionResult AddShowingDate(int movieId, int theatreId, DateTime SelectedDate)
        {
            var defaultTimes = _dbc.ShowingTimes.Where(s => s.TheatreId == theatreId && s.ShowingDate == null).Select(s => s.ShowingDatetime).ToList();

            if (!defaultTimes.Any())
            {
                TempData["Error"] = "Rạp này chưa có giờ chiếu mặc định. Vui lòng thêm giờ chiếu trước.";
                return RedirectToAction("ManageTheatres", new { movieId });
            }

            foreach (var time in defaultTimes)
            {
                if (!_dbc.ShowingTimes.Any(s => s.TheatreId == theatreId && s.ShowingDate == SelectedDate.Date && s.ShowingDatetime == time))
                {
                    _dbc.ShowingTimes.Add(new ShowingTime { TheatreId = theatreId, ShowingDate = SelectedDate.Date, ShowingDatetime = time });
                }
            }

            _dbc.SaveChanges();
            TempData["Success"] = $"Đã thêm ngày chiếu {SelectedDate:dd/MM/yyyy} thành công!";
            return RedirectToAction("ManageShowingDates", new { movieId, theatreId });
        }
    }
}
