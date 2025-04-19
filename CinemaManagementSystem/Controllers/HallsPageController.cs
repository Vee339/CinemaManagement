using Microsoft.AspNetCore.Mvc;
using CinemaManagementSystem.Models;
using CinemaManagementSystem.Interfaces;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CinemaManagementSystem.Controllers
{
    public class HallsPageController : Controller
    {
        private readonly IHallsService _hallsService;
        private readonly IScreeningsService _screeningsService;

        public HallsPageController(IHallsService HallsService, IScreeningsService screeningsService)
        {
            _hallsService = HallsService;
            _screeningsService = screeningsService;
        }
        public IActionResult Index()
        {
            return RedirectToAction("List");
        }

        // Get: HallsPage/List
        public async Task<IActionResult> List()
        {
            IEnumerable<Hall> Halls = await _hallsService.GetHalls();

            return View(Halls);
        }
    }
}
