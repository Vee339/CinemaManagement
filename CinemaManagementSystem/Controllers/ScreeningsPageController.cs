using CinemaManagementSystem.Interfaces;
using CinemaManagementSystem.Models;
using CinemaManagementSystem.Models.ViewsModels;
using Microsoft.AspNetCore.Mvc;

namespace CinemaManagementSystem.Controllers
{
    public class ScreeningsPageController : Controller
    {
        private readonly IScreeningsService _screeningsService;
        private readonly IMoviesService _moviesService;
        private readonly IHallsService _hallsService;

        public ScreeningsPageController(IScreeningsService ScreeningsService, IMoviesService MoviesService, IHallsService HallsService)
        {
            _screeningsService = ScreeningsService;
            _moviesService = MoviesService;
            _hallsService = HallsService;
        }

        // GET -> /ScreeningsPage
        [HttpGet]
        public IActionResult Index()
        {
            return RedirectToAction("List");
        }

        // GET -> /ScreeningsPage/List
        [HttpGet]
        public async Task<IActionResult> List()
        {
            IEnumerable<ScreeningDto> ScreeningDto = await _screeningsService.GetScreenings();

            return View(ScreeningDto);
        }

        // GET -> /ScreeningsPage/Details/{id}
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            ScreeningDto Screening = await _screeningsService.FindScreening(id);
            return View(Screening);
        }

        // GET -> /ScreeningsPage/New
        [HttpGet]
        public async Task<IActionResult> New()
        {
            IEnumerable<MovieDto> MoviesList = await _moviesService.GetMovies();
            IEnumerable<Hall> HallsList = await _hallsService.GetHalls();

            MoviesHalls moviesHalls = new MoviesHalls()
            {
                Movies = MoviesList,
                Halls = HallsList
            };

            return View(moviesHalls);

        }

        // POST -> /ScreeningsPage/Add
        [HttpPost]
        public async Task<IActionResult> Add(Screening screening)
        {
            await _screeningsService.AddScreening(screening);
            return RedirectToAction("List", "ScreeningsPage");
        }

        // GET -> /ScreeningPage/Edit/{id}
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            ScreeningDto screening = await _screeningsService.FindScreening(id);
            IEnumerable<MovieDto> MoviesList = await _moviesService.GetMovies();
            IEnumerable<Hall> HallsList = await _hallsService.GetHalls();

            MoviesHalls moviesHalls = new MoviesHalls()
            {
                Screening = screening,
                Movies = MoviesList,
                Halls = HallsList
            };

            return View(moviesHalls);
        }

        // POST -> /ScreeningsPage/Update/{id}
        [HttpPost]
        public async Task<IActionResult> Update(int id, Screening screening)
        {
            await _screeningsService.EditScreening(id, screening);
            return RedirectToAction("Details", "ScreeningsPage", new { id = id});
        }

        // GET -> /ScreeningsPage/ConfirmDelete/{id}
        [HttpGet]
        public async Task<IActionResult> ConfirmDelete(int id)
        {
            ScreeningDto Screening = await _screeningsService.FindScreening(id);
            return View(Screening);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _screeningsService.DeleteScreening(id);
            return RedirectToAction("List", "ScreeningsPage");
        }

    }
}
