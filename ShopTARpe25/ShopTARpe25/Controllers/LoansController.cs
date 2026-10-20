using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ShopTARpe25.Core.Domain;
using ShopTARpe25.Core.ServiceInterface;
using ShopTARpe25.Data;

namespace ShopTARpe25.Controllers
{
    public class LoansController : Controller
    {
        private readonly ILoanServices _loanServices;
        private readonly ShopTARpe25Context _context;

        public LoansController(
            ILoanServices loanServices,
            ShopTARpe25Context context)
        {
            _loanServices = loanServices;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var loans = await _loanServices.GetAll();
            return View(loans);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            await LoadData();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Loan loan)
        {
            ModelState.Remove(nameof(loan.Book));
            ModelState.Remove(nameof(loan.Reader));

            if (!ModelState.IsValid)
            {
                await LoadData();
                return View(loan);
            }

            try
            {
                await _loanServices.Create(loan);
                return RedirectToAction(nameof(Index));
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError("", ex.Message);
                await LoadData();
                return View(loan);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ReturnBook(Guid id)
        {
            bool returned = await _loanServices.ReturnBook(id);

            if (!returned)
                TempData["LoanError"] = "Laenutust ei leitud või raamat on juba tagastatud.";

            return RedirectToAction(nameof(Index));
        }

        private async Task LoadData()
        {
            var books = await _context.Books
                .Where(b => b.IsAvailable)
                .OrderBy(b => b.Title)
                .ToListAsync();

            var readers = await _context.Readers
                .OrderBy(r => r.FullName)
                .ToListAsync();

            ViewBag.Books = new SelectList(books, "Id", "Title");
            ViewBag.Readers = new SelectList(readers, "Id", "FullName");
        }
    }
}
