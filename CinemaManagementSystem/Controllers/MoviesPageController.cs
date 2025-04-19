using Microsoft.AspNetCore.Mvc;
using CinemaManagementSystem.Interfaces;
using CinemaManagementSystem.Models;
using CinemaManagementSystem.Services;
using System.ComponentModel;
using CinemaManagementSystem.Models.ViewsModels;

namespace CinemaManagementSystem.Controllers
{
    public class MoviesPageController : Controller
    {

        private readonly IMoviesService _moviesService;
        private readonly IScreeningsService _screeningsService;

        public MoviesPageController(IMoviesService MoviesService, IScreeningsService ScreeningsService)
        {
            _moviesService = MoviesService;
            _screeningsService = ScreeningsService;
        }
        public IActionResult Index()
        {
            return RedirectToAction("List");
        }

        // GET: /MoviesPage/List
        public async Task<IActionResult> List()
        {
            IEnumerable<MovieDto?> MovieDtos = await _moviesService.GetMovies();
            return View(MovieDtos);
        }

        // GET: /MoviesPage/Details/{id}
        public async Task<IActionResult> Details(int id)
        {
            MovieDto? MovieDto = await _moviesService.FindMovie(id);

            IEnumerable<ScreeningDto> AssociatedScreenings = await _screeningsService.ListScreeningsForMovie(id);

            MovieDetails MovieInfo = new MovieDetails
            {
                Movie = MovieDto,
                Screenings = AssociatedScreenings
            };

            return View(MovieInfo);
        }

        // GET: /MoviesPage/New
        [HttpGet]
        public IActionResult New()
        {
            return View();
        }

        // POST: /MoviesPage/Add
        [HttpPost]
        public async Task<IActionResult> Add(Movie movie)
        {
            await _moviesService.AddMovie(movie);

            return RedirectToAction("List", "MoviesPage");
        }

        // GET : /MoviesPage/Edit/{id}
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            Movie movie = await _moviesService.FindMovieDetails(id);

            return View(movie);
        }

        // POST: /MoviesPage/Update/{id}
        [HttpPost]
        public async Task<IActionResult> Update(int id, Movie movie)
        {
            await _moviesService.PutMovie(id, movie);

            return RedirectToAction("Details", "MoviesPage", new { id = id });
        }

        // GET: /MoviesPage/ConfirmDelete/{id}
        [HttpGet]
        public async Task<IActionResult> ConfirmDelete(int id)
        {
            Movie movie = await _moviesService.FindMovieDetails(id);
            return View(movie);
        }

        // POST: /MoviesPage/Delete/{id}
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _moviesService.DeleteMovie(id);
            return RedirectToAction("List", "MoviesPage");
        }
        
    }
}

