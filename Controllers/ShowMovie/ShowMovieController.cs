using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CMS.Models;

namespace CMS.Controllers.ShowMovie
{
    public class ShowMovieController : Controller
    {
        private readonly CinemaDbContext _dbc;

        public ShowMovieController(CinemaDbContext db)
        {
            _dbc = db;
        }

        // GET: /ShowMovie/Movie
        public IActionResult Index(string searchString, int? statusId)
        {
            var moviesQuery = _dbc.Movies
                .Include(m => m.Status)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                moviesQuery = moviesQuery.Where(m => m.Title.Contains(searchString));
            }


            if (statusId.HasValue)
            {
                moviesQuery = moviesQuery.Where(m => m.StatusId == statusId);
            }

            var viewModel = new MovieListViewModel
            {
                Movies = moviesQuery.ToList(),
                Genres = _dbc.Genres.ToList(),
                Statuses = _dbc.MovieStatuses.ToList(),
                SelectedStatusId = statusId,
                SearchKeyword = searchString
            };

            return View(viewModel);
        }

    }
}
