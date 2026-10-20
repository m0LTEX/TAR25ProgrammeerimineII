using ShopTARpe25.Core.Domain;

namespace ShopTARpe25.Core.ServiceInterface
{
    public interface ILoanServices
    {
        Task<List<Loan>> GetAll();
        Task<Loan?> GetById(Guid id);
        Task<Loan> Create(Loan loan);
        Task<bool> ReturnBook(Guid id);
    }
}
