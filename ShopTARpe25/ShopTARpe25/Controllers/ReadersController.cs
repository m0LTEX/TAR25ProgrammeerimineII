
using Microsoft.AspNetCore.Mvc;
using ShopTARpe25.Core.Domain;
using ShopTARpe25.Core.ServiceInterface;

namespace ShopTARpe25.Controllers
{
    public class ReadersController : Controller
    {
        private readonly IReaderServices _readerServices;

        public ReadersController(IReaderServices readerServices)
        {
            _readerServices = readerServices;
        }

        public async Task<IActionResult> Index(string? search)
        {
            ViewBag.Search = search;

            var readers = await _readerServices.GetAll(search);

            return View(readers);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Reader reader)
        {
            if (!ModelState.IsValid)
                return View(reader);

            await _readerServices.Create(reader);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var reader = await _readerServices.GetById(id);

            if (reader == null)
                return NotFound();

            return View(reader);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Reader reader)
        {
            if (!ModelState.IsValid)
                return View(reader);

            bool updated = await _readerServices.Update(reader);

            if (!updated)
                return NotFound();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            var reader = await _readerServices.GetById(id);

            if (reader == null)
                return NotFound();

            return View(reader);
        }

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            bool deleted = await _readerServices.Delete(id);

            if (!deleted)
            {
                TempData["ReaderError"] =
                    "Lugejat ei leitud või temaga on seotud laenutused.";

                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
