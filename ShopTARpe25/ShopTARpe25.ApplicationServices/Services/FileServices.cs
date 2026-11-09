
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using ShopTARpe25.Core.Domain;
using ShopTARpe25.Core.Dto;
using ShopTARpe25.Core.ServiceInterface;
using ShopTARpe25.Data;

namespace ShopTARpe25.ApplicationServices.Services
{
    public class FileServices : IFileServices
    {
        private readonly ShopTARpe25Context _context;
        private readonly IHostEnvironment _webHost;

        public FileServices
        (
            ShopTARpe25Context context,
            IHostEnvironment webHost
        )
        {
            _context = context;
            _webHost = webHost;
        }

        // SPACESHIP
        public void FilesToApi(SpaceshipDto dto, Spaceship domain)
        {
            SaveFiles(dto.Files, domain.Id, null);
        }

        // REALESTATE
        public void FilesToApi(RealEstateDto dto, RealEstate domain)
        {
            SaveFiles(dto.Files, null, domain.Id);
        }

        // COMMON METHOD FOR SAVING FILES
        private void SaveFiles(
            IEnumerable<Microsoft.AspNetCore.Http.IFormFile>? files,
            Guid? spaceshipId,
            Guid? realEstateId)
        {
            if (files == null)
            {
                return;
            }

            string uploadsFolder = Path.Combine(
                _webHost.ContentRootPath,
                "wwwroot",
                "multipleFileUpload"
            );

            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            foreach (var file in files)
            {
                if (file.Length <= 0)
                {
                    continue;
                }

                string uniqueFileName =
                    Guid.NewGuid().ToString() + "_" + Path.GetFileName(file.FileName);

                string filePath = Path.Combine(
                    uploadsFolder,
                    uniqueFileName
                );

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }

                FileToApi path = new FileToApi
                {
                    Id = Guid.NewGuid(),
                    ExistingFilePath = uniqueFileName,
                    SpaceshipId = spaceshipId,
                    RealEstateId = realEstateId
                };

                _context.FileToApis.Add(path);
            }
        }

        // REMOVE ONE IMAGE
        public async Task<FileToApi> RemoveImageFromApi(FileToApiDto dto)
        {
            var image = await _context.FileToApis
                .FirstOrDefaultAsync(x => x.Id == dto.Id);

            if (image == null)
            {
                return null;
            }

            string filePath = Path.Combine(
                _webHost.ContentRootPath,
                "wwwroot",
                "multipleFileUpload",
                image.ExistingFilePath ?? ""
            );

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            _context.FileToApis.Remove(image);

            await _context.SaveChangesAsync();

            return image;
        }

        // REMOVE MULTIPLE IMAGES
        public async Task<List<FileToApi>> RemoveImagesFromApi(FileToApiDto[] dto)
        {
            List<FileToApi> removedImages = new();

            foreach (var item in dto)
            {
                var image = await _context.FileToApis
                    .FirstOrDefaultAsync(x => x.Id == item.Id);

                if (image == null)
                {
                    continue;
                }

                string filePath = Path.Combine(
                    _webHost.ContentRootPath,
                    "wwwroot",
                    "multipleFileUpload",
                    image.ExistingFilePath ?? ""
                );

                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }

                _context.FileToApis.Remove(image);
                removedImages.Add(image);
            }

            await _context.SaveChangesAsync();

            return removedImages;
        }
    }
}
