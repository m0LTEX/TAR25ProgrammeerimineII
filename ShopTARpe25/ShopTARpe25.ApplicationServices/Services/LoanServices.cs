using Microsoft.EntityFrameworkCore;
using ShopTARpe25.Core.Domain;
using ShopTARpe25.Core.ServiceInterface;
using ShopTARpe25.Data;

namespace ShopTARpe25.ApplicationServices.Services
{
    public class LoanServices : ILoanServices
    {
        private readonly ShopTARpe25Context _context;

        public LoanServices(ShopTARpe25Context context)
        {
            _context = context;
        }

        public async Task<List<Loan>> GetAll()
        {
            return await _context.Loans
                .Include(l => l.Book)
                .Include(l => l.Reader)
                .OrderByDescending(l => l.LoanDate)
                .ToListAsync();
        }

        public async Task<Loan?> GetById(Guid id)
        {
            return await _context.Loans
                .Include(l => l.Book)
                .Include(l => l.Reader)
                .FirstOrDefaultAsync(l => l.Id == id);
        }

        public async Task<Loan> Create(Loan loan)
        {
            var book = await _context.Books
                .FirstOrDefaultAsync(b => b.Id == loan.BookId);

            var reader = await _context.Readers
                .FirstOrDefaultAsync(r => r.Id == loan.ReaderId);

            if (book == null || reader == null)
                throw new InvalidOperationException("Raamatut või lugejat ei leitud.");

            if (!book.IsAvailable)
                throw new InvalidOperationException("Raamat ei ole saadaval.");

            loan.Id = Guid.NewGuid();
            loan.LoanDate = DateTime.Now;
            loan.ReturnDate = null;

            book.IsAvailable = false;

            await _context.Loans.AddAsync(loan);
            await _context.SaveChangesAsync();

            return loan;
        }

        public async Task<bool> ReturnBook(Guid id)
        {
            var loan = await _context.Loans
                .Include(l => l.Book)
                .FirstOrDefaultAsync(l => l.Id == id);

            if (loan == null || loan.ReturnDate != null)
                return false;

            loan.ReturnDate = DateTime.Now;
            loan.Book.IsAvailable = true;

            await _context.SaveChangesAsync();

            return true;
        }
    }
}
