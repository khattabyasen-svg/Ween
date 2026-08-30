using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Ween.Models;
using Ween.Services;

namespace Ween.Controllers
{
    public class HomeController : Controller
    {
        private readonly ICityService _cityService;

        public HomeController(ICityService cityService)
        {
            _cityService = cityService;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _cityService.GetCityCardsAsync());
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
