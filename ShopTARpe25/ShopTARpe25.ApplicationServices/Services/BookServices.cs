
using Microsoft.EntityFrameworkCore;
using ShopTARpe25.Core.Domain;
using ShopTARpe25.Core.ServiceInterface;
using ShopTARpe25.Data;

namespace ShopTARpe25.ApplicationServices.Services
{
    public class BookServices : IBookServices
    {
        private readonly ShopTARpe25Context _context;

        public BookServices(ShopTARpe25Context context)
        {
            _context = context;
        }

        public async Task<Book> Create(Book book)
        {
            book.Id = Guid.NewGuid();
            book.IsAvailable = true;

            await _context.Books.AddAsync(book);
            await _context.SaveChangesAsync();

            return book;
        }

        public async Task<List<Book>> GetAll(
            string? title,
            int? year,
            bool? isAvailable)
        {
            var query = _context.Books.AsQueryable();

            if (!string.IsNullOrWhiteSpace(title))
            {
                query = query.Where(b => b.Title.Contains(title));
            }

            if (year.HasValue)
            {
                query = query.Where(
                    b => b.PublishedYear == year.Value);
            }

            if (isAvailable.HasValue)
            {
                query = query.Where(
                    b => b.IsAvailable == isAvailable.Value);
            }

            return await query
                .OrderBy(b => b.Title)
                .ToListAsync();
        }
    }
}
