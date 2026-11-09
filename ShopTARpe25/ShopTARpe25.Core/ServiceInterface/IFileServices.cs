using ShopTARpe25.Core.Domain;
using ShopTARpe25.Core.Dto;

namespace ShopTARpe25.Core.ServiceInterface
{
    public interface IFileServices
    {
        // Spaceship
        void FilesToApi(SpaceshipDto dto, Spaceship domain);

        // RealEstate
        void FilesToApi(RealEstateDto dto, RealEstate domain);

        Task<FileToApi> RemoveImageFromApi(FileToApiDto dto);

        Task<List<FileToApi>> RemoveImagesFromApi(FileToApiDto[] dto);
    }
}