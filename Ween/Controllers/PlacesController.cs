using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ween.Extensions;
using Ween.Models;
using Ween.Services;

namespace Ween.Controllers
{
    public class PlacesController : Controller
    {
        private readonly IPlaceService _placeService;
        private readonly IReservationService _reservationService;

        public PlacesController(IPlaceService placeService, IReservationService reservationService)
        {
            _placeService = placeService;
            _reservationService = reservationService;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var model = await _placeService.GetDetailsAsync(id);
            if (model is null)
                return NotFound();

            return View(model);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Reserve(ReservationInputModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["ResError"] = "Please fill in the date, time and party size.";
                return RedirectToAction(nameof(Details), new { id = model.PlaceId });
            }

            var userId = User.GetUserId();
            if (userId is null)
                return Challenge();

            var result = await _reservationService.CreateAsync(userId.Value, model);

            if (!result.Ok)
                TempData["ResError"] = result.Error;
            else
                TempData["ResConfirm"] =
                    $"Reservation confirmed for {model.ReservationDate:yyyy-MM-dd} at {model.ReservationTime:HH:mm} " +
                    $"({model.PartySize}). {result.Remaining} spots left that day.";

            return RedirectToAction(nameof(Details), new { id = model.PlaceId });
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> DayUsage(int placeId, DateOnly date)
        {
            var (capacity, booked, remaining) = await _reservationService.GetDayUsageAsync(placeId, date);
            return Json(new { capacity, booked, remaining });
        }
    }
}
