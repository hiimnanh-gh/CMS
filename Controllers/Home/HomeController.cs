using Microsoft.AspNetCore.Mvc;
using CMS.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System;

namespace test_2.Controllers.Home
{
    public class HomeController : Controller
    {
        private readonly CinemaDbContext _dbc;

        public HomeController(CinemaDbContext db)
        {
            _dbc = db;
        }

        // ========== HIỂN THỊ DANH SÁCH PHIM ==========
        public IActionResult Index(string search, int page = 1)
        {
            int pageSize = 6; // Số phim mỗi trang
            var query = _dbc.Movies
                .Include(m => m.Director)
                .Include(m => m.Status)
                .AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(m => m.Title.Contains(search));
                ViewData["SearchQuery"] = search;
            }

            int totalMovies = query.Count(); // Tổng số phim
            int totalPages = (int)Math.Ceiling((double)totalMovies / pageSize);

            var movies = query.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            // 🔹 Đảm bảo `Model` luôn có giá trị, tránh lỗi NullReferenceException
            if (movies == null)
            {
                movies = new List<Movie>();
            }

            ViewData["CurrentPage"] = page;
            ViewData["TotalPages"] = totalPages;

            return View(movies);
        }

        public IActionResult Privacy()
        {
            return View();
        }

    }
}