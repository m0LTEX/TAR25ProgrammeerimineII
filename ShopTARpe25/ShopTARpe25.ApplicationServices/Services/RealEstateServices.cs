
using Microsoft.EntityFrameworkCore;
using ShopTARpe25.Core.Domain;
using ShopTARpe25.Core.Dto;
using ShopTARpe25.Core.ServiceInterface;
using ShopTARpe25.Data;

namespace ShopTARpe25.ApplicationServices.Services
{
    public class RealEstateServices : IRealEstateServices
    {
        private readonly ShopTARpe25Context _context;
        private readonly IFileServices _fileServices;

        public RealEstateServices
        (
            ShopTARpe25Context context,
            IFileServices fileServices
        )
        {
            _context = context;
            _fileServices = fileServices;
        }

        // CREATE
        public async Task<RealEstate> Create(RealEstateDto dto)
        {
            RealEstate domain = new RealEstate();

            domain.Id = dto.Id ?? Guid.NewGuid();
            domain.Area = dto.Area;
            domain.Address = dto.Address;
            domain.RoomNumber = dto.RoomNumber;
            domain.BuildingType = dto.BuildingType;
            domain.CreatedAt = DateTime.Now;
            domain.ModifiedAt = DateTime.Now;

            _fileServices.FilesToApi(dto, domain);

            await _context.Realestates.AddAsync(domain);
            await _context.SaveChangesAsync();

            return domain;
        }

        // DETAILS
        public async Task<RealEstate> DetailAsync(Guid id)
        {
            var result = await _context.Realestates
                .FirstOrDefaultAsync(x => x.Id == id);

            return result;
        }

        // UPDATE
        public async Task<RealEstate> Update(RealEstateDto dto)
        {
            var realEstate = await _context.Realestates
                .FirstOrDefaultAsync(x => x.Id == dto.Id);

            if (realEstate == null)
            {
                return null;
            }

            realEstate.Area = dto.Area;
            realEstate.Address = dto.Address;
            realEstate.RoomNumber = dto.RoomNumber;
            realEstate.BuildingType = dto.BuildingType;
            realEstate.ModifiedAt = DateTime.Now;

            _fileServices.FilesToApi(dto, realEstate);

            _context.Realestates.Update(realEstate);
            await _context.SaveChangesAsync();

            return realEstate;
        }

        // DELETE
        public async Task<RealEstate> Delete(Guid id)
        {
            var result = await _context.Realestates
                .FirstOrDefaultAsync(x => x.Id == id);

            if (result == null)
            {
                return null;
            }

            var images = await _context.FileToApis
                .Where(x => x.RealEstateId == id)
                .Select(y => new FileToApiDto
                {
                    Id = y.Id,
                    RealEstateId = y.RealEstateId,
                    ExistingFilePath = y.ExistingFilePath
                })
                .ToArrayAsync();

            await _fileServices.RemoveImagesFromApi(images);

            _context.Realestates.Remove(result);

            await _context.SaveChangesAsync();

            return result;
        }
    }
}
