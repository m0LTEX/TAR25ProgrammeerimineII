using ShopTARpe25.Core.Domain;

namespace ShopTARpe25.Core.ServiceInterface
{
    public interface IReaderServices
    {
        Task<Reader> Create(Reader reader);
        Task<List<Reader>> GetAll(string? search);
        Task<Reader?> GetById(Guid id);
        Task<bool> Update(Reader reader);
        Task<bool> Delete(Guid id);
    }
}
