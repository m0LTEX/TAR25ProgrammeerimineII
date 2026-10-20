using ShopTARpe25.Core.Domain;

namespace ShopTARpe25.Core.ServiceInterface
{
    public interface IBookServices
    {
        Task<Book> Create(Book book);

        Task<List<Book>> GetAll(
            string? title,
            int? year,
            bool? isAvailable);
    }
}
