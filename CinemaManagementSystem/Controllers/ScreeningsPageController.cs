using CinemaManagementSystem.Interfaces;
using CinemaManagementSystem.Models;
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
        public IActionResult Index()
        {
            return RedirectToAction("List");
        }

        public async Task<IActionResult> List()
        {
            IEnumerable<ScreeningDto> ScreeningDto = await _screeningsService.GetScreenings();

            return View(ScreeningDto);
        }
    }
}
