using ShopTARpe25.Core.Domain;
using ShopTARpe25.Core.Dto;


namespace ShopTARpe25.Core.ServiceInterface
{
    public interface ISpaceshipServices
    {
        Task<Reader> Create(SpaceshipDto dto);
        Task<Reader> DetailsAsync(Guid id);
        Task<Reader> Update(SpaceshipDto dto);
        Task<Reader> Delete(Guid id);
    }
}
