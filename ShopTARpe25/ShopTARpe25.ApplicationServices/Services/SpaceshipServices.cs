
using Microsoft.EntityFrameworkCore;
using ShopTARpe25.Core.Domain;
using ShopTARpe25.Core.Dto;
using ShopTARpe25.Core.ServiceInterface;
using ShopTARpe25.Data;

namespace ShopTARpe25.ApplicationServices.Services
{
    public class SpaceshipServices : ISpaceshipServices
    {
        private readonly ShopTARpe25Context _context;
        private readonly IFileServices _fileServices;

        public SpaceshipServices(
            ShopTARpe25Context context,
            IFileServices fileServices)
        {
            _context = context;
            _fileServices = fileServices;
        }

        public async Task<Spaceship> Create(SpaceshipDto dto)
        {
            Spaceship domain = new()
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                Classification = dto.Classification,
                BuiltDate = dto.BuiltDate,
                Crew = dto.Crew,
                EnginePower = dto.EnginePower,
                CreatedAt = DateTime.Now,
                ModifiedAt = DateTime.Now
            };

            _fileServices.FilesToApi(dto, domain);

            await _context.Spaceships.AddAsync(domain);
            await _context.SaveChangesAsync();

            return domain;
        }

        public async Task<Spaceship> DetailsAsync(Guid id)
        {
            return await _context.Spaceships
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Spaceship> Update(SpaceshipDto dto)
        {
            var spaceship = await _context.Spaceships
                .FirstOrDefaultAsync(x => x.Id == dto.Id);

            if (spaceship == null)
                return null;

            spaceship.Name = dto.Name;
            spaceship.Classification = dto.Classification;
            spaceship.BuiltDate = dto.BuiltDate;
            spaceship.Crew = dto.Crew;
            spaceship.EnginePower = dto.EnginePower;
            spaceship.ModifiedAt = DateTime.Now;

            _fileServices.FilesToApi(dto, spaceship);

            await _context.SaveChangesAsync();

            return spaceship;
        }

        public async Task<Spaceship> Delete(Guid id)
        {
            var spaceship = await _context.Spaceships
                .FirstOrDefaultAsync(x => x.Id == id);

            if (spaceship == null)
                return null;

            var images = await _context.FileToApis
                .Where(x => x.SpaceshipId == id)
                .Select(x => new FileToApiDto
                {
                    Id = x.Id,
                    SpaceshipId = x.SpaceshipId,
                    ExistingFilePath = x.ExistingFilePath
                })
                .ToArrayAsync();

            await _fileServices.RemoveImagesFromApi(images);

            _context.Spaceships.Remove(spaceship);
            await _context.SaveChangesAsync();

            return spaceship;
        }
    }
}
