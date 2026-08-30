using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ween.Extensions;
using Ween.Models;
using Ween.Services;

namespace Ween.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IAdminService _adminService;

        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View(await _adminService.GetDashboardAsync());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddCity(AddCityInputModel model)
        {
            if (!ModelState.IsValid)
                TempData["AdminMsg"] = "City name is required.";
            else
            {
                await _adminService.AddCityAsync(model.Name, model.LocalName);
                TempData["AdminMsg"] = $"City \"{model.Name}\" added.";
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddCategory(AddCategoryInputModel model)
        {
            if (!ModelState.IsValid)
                TempData["AdminMsg"] = "Category name is required.";
            else
            {
                await _adminService.AddCategoryAsync(model.Name, model.Icon);
                TempData["AdminMsg"] = $"Category \"{model.Name}\" added.";
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePlace(CreatePlaceInputModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["AdminMsg"] = "Please fill in the required place fields (city, category, name, address).";
                return RedirectToAction(nameof(Index));
            }

            var userId = User.GetUserId();
            if (userId is null)
                return Challenge();

            var name = await _adminService.CreatePlaceAsync(model, userId.Value);
            TempData["AdminMsg"] = name is null
                ? "That city or category no longer exists — please pick from the lists."
                : $"\"{name}\" was registered and now appears in its city's listings.";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> CategoryFields(int categoryId)
        {
            return Json(await _adminService.GetCategoryFieldsAsync(categoryId));
        }

        [HttpGet]
        public async Task<IActionResult> Reservations(string? filter)
        {
            return View(await _adminService.GetReservationsAsync(filter));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CancelReservation(int id, string? filter)
        {
            await _adminService.CancelReservationAsync(id);
            return RedirectToAction(nameof(Reservations), new { filter });
        }
    }
}
