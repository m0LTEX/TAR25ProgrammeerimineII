using Microsoft.AspNetCore.Mvc;
using ShopTARpe25.Core.Domain;
using ShopTARpe25.Core.ServiceInterface;

namespace ShopTARpe25.Controllers
{
    public class BooksController : Controller
    {
        private readonly IBookServices _bookServices;

        public BooksController(IBookServices bookServices)
        {
            _bookServices = bookServices;
        }

        [HttpGet]
        public async Task<IActionResult> Index(
            string? title,
            int? year,
            bool? isAvailable)
        {
            ViewBag.TitleSearch = title;
            ViewBag.YearSearch = year;
            ViewBag.AvailabilitySearch = isAvailable;

            var books = await _bookServices.GetAll(
                title, year, isAvailable);

            return View(books);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Book book)
        {
            if (!ModelState.IsValid)
            {
                return View(book);
            }

            await _bookServices.Create(book);

            return RedirectToAction(nameof(Index));
        }
    }
}
