using Microsoft.AspNetCore.Mvc;
using CMS.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System;

namespace CMS.Controllers.MovieC
{
    public class MovieController : Controller
    {
        private readonly CinemaDbContext _dbc;

        public MovieController(CinemaDbContext db)
        {
            _dbc = db;
        }

        // ========= XEM CHI TIẾT PHIM ==========
        public IActionResult Details(int id)
        {
            var movie = _dbc.Movies
                .Include(m => m.Director)
                .Include(m => m.Status)
                .FirstOrDefault(m => m.MovieId == id);

            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }
    }
}
