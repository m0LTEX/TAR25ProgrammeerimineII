using ShopTARpe25.Core.Domain;
using ShopTARpe25.Core.Dto;

namespace ShopTARpe25.Core.ServiceInterface
{
    public interface IFileServices
    {
        void FilesToApi(SpaceshipDto dto, Reader domain);
        Task<Book> RemoveImageFromApi(FileToApiDto dto);
        Task<List<Book>> RemoveImagesFromApi(FileToApiDto[] dto);
    }
}
