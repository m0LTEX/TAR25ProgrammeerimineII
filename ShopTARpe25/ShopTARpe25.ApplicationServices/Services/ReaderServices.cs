
using Microsoft.EntityFrameworkCore;
using ShopTARpe25.Core.Domain;
using ShopTARpe25.Core.ServiceInterface;
using ShopTARpe25.Data;

namespace ShopTARpe25.ApplicationServices.Services
{
    public class ReaderServices : IReaderServices
    {
        private readonly ShopTARpe25Context _context;

        public ReaderServices(ShopTARpe25Context context)
        {
            _context = context;
        }

        public async Task<Reader> Create(Reader reader)
        {
            reader.Id = Guid.NewGuid();

            await _context.Readers.AddAsync(reader);
            await _context.SaveChangesAsync();

            return reader;
        }

        public async Task<List<Reader>> GetAll(string? search)
        {
            var query =
                from r in _context.Readers
                where string.IsNullOrWhiteSpace(search)
                      || r.FullName.Contains(search!)
                      || r.Email.Contains(search!)
                orderby r.FullName
                select r;

            return await query.ToListAsync();
        }

        public async Task<Reader?> GetById(Guid id)
        {
            return await _context.Readers
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<bool> Update(Reader reader)
        {
            var existing = await _context.Readers
                .FirstOrDefaultAsync(r => r.Id == reader.Id);

            if (existing == null)
                return false;

            existing.FullName = reader.FullName;
            existing.Email = reader.Email;

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> Delete(Guid id)
        {
            var reader = await _context.Readers
                .FirstOrDefaultAsync(r => r.Id == id);

            if (reader == null)
                return false;

            bool hasLoans = await _context.Loans
                .AnyAsync(l => l.ReaderId == id);

            if (hasLoans)
                return false;

            _context.Readers.Remove(reader);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
