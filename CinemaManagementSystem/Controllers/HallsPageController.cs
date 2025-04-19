using Microsoft.AspNetCore.Mvc;
using CinemaManagementSystem.Models;
using CinemaManagementSystem.Interfaces;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using CinemaManagementSystem.Models.ViewsModels;

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
        [HttpGet]
        public async Task<IActionResult> List()
        {
            IEnumerable<Hall> Halls = await _hallsService.GetHalls();

            return View(Halls);
        }

        // Get: /HallsPage/Details/{id}
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            Hall Hall = await _hallsService.FindHall(id);
            IEnumerable<ScreeningDto> Screenings = await _screeningsService.ListScreeningsForHall(id);

            HallDetails HallInfo = new HallDetails()
            {
                Hall = Hall,
                HallScreenings = Screenings
            };

            return View(HallInfo);
        }

        // Get: /HallsPage/New
        [HttpGet]
        public IActionResult New()
        {
            return View();
        }

        // Post: /HallsPage/Add
        [HttpPost]
        public async Task<IActionResult> Add(Hall hall)
        {
            await _hallsService.AddHall(hall);
            return RedirectToAction("List", "HallsPage");
        }

        // Get: /HallsPage/Edit
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            Hall hall = await _hallsService.FindHall(id);
            return View(hall);
        }

        // POST: /HallsPage/Update/{id}
        [HttpPost]
        public async Task<IActionResult> Update(int id, Hall hall)
        {
            await _hallsService.EditHall(id, hall);
            return RedirectToAction("Details", "HallsPage", new { id= id });
        }

        // Get: /HallsPage/ConfirmDelete/{id}
        [HttpGet]
        public async Task<IActionResult> ConfirmDelete(int id)
        {
            Hall hall = await _hallsService.FindHall(id);
            return View(hall);
        }

        // Post: /HallsPage/Delete/{id}
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _hallsService.DeleteHall(id);
            return RedirectToAction("List", "HallsPage");
        }
    }
}
