using Microsoft.AspNetCore.Mvc;
using Ween.Services;

namespace Ween.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public async Task<IActionResult> Index(string city)
        {
            if (string.IsNullOrWhiteSpace(city))
                return RedirectToAction("Index", "Home");

            var model = await _categoryService.GetCategoriesForCityAsync(city);
            if (model is null)
                return NotFound();

            return View(model);
        }
    }
}
