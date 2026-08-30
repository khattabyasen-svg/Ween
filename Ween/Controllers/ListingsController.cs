using Microsoft.AspNetCore.Mvc;
using Ween.Services;

namespace Ween.Controllers
{
    public class ListingsController : Controller
    {
        private readonly IListingService _listingService;

        public ListingsController(IListingService listingService)
        {
            _listingService = listingService;
        }

        public async Task<IActionResult> Index(string city, string category)
        {
            if (string.IsNullOrWhiteSpace(city))
                return RedirectToAction("Index", "Home");

            if (string.IsNullOrWhiteSpace(category))
                return RedirectToAction("Index", "Categories", new { city });

            var model = await _listingService.GetListingsAsync(city, category);
            if (model is null)
                return NotFound();

            return View("Listings", model);
        }
    }
}
