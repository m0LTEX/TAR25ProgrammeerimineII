using Microsoft.EntityFrameworkCore;
using ShopTARpe25.Core.Domain;
using ShopTARpe25.Core.Dto;
using ShopTARpe25.Core.ServiceInterface;
using ShopTARpe25.Data;
using System;
using System.Collections.Generic;
using System.Text;

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

        public async Task<RealEstate> Create(RealEstateDto dto)
        {
            RealEstate domain = new();

            domain.Id = Guid.NewGuid();
            domain.Area = dto.Area;
            domain.Address = dto.Address;
            domain.RoomNumber = dto.RoomNumber;
            domain.BuildingType = dto.BuildingType;
            domain.CreatedAt = DateTime.Now;
            domain.ModifiedAt = DateTime.Now;
            //peame saama File teenuse välja kutsuda,
            //mis salvestab failid serverisse
            _fileServices.FilesToApi(dto, domain);


            //siia tuleb kood, mis salvestab domain
            //objekti andmebaasi
            //tuleb kasutada repository'd, mis
            //on defineeritud Core projektis
            //konstruktori kaudu tuleb injectida repository

            await _context.Realestates.AddAsync(domain);
            await _context.SaveChangesAsync();

            return domain;
        }

        //siia teha meetod nimega DetailsAsync
        //see ainult p'rib andmed contextist
        public async Task<RealEstate> DetailsAsync(Guid id)
        {
            var result = await _context.Realestates
                .FirstOrDefaultAsync(x => x.Id == id);

            return result;
        }

        public async Task<RealEstate> Update(RealEstateDto dto)
        {
            RealEstate realEstate = new();

            realEstate.Id = dto.Id;
            realEstate.Area = dto.Area;
            realEstate.Address = dto.Address;
            realEstate.RoomNumber = dto.RoomNumber;
            realEstate.BuildingType = dto.BuildingType;
            realEstate.CreatedAt = dto.CreatedAt;
            realEstate.ModifiedAt = DateTime.Now;
            _fileServices.FilesToApi(dto, realEstate);

            _context.Realestates.Update(realEstate);
            await _context.SaveChangesAsync();

            return realEstate;
        }

        public async Task<RealEstate> Delete(Guid id)
        {
            var result = await _context.Realestates
                .FirstOrDefaultAsync(x => x.Id == id);

            var images = await _context.FileToApis
                .Where(x => x.RealEstateId == id)
                .Select(y => new FileToApiDto
                {
                    Id = y.Id,
                    RealEstateId = y.RealEstateId,
                    ExistingFilePath = y.ExistingFilePath,
                }).ToArrayAsync();

            //peame kutsuma meetodi esile, mis kustutab failid serverist
            await _fileServices.RemoveImagesFromApi(images);
            _context.Realestates.Remove(result);
            await _context.SaveChangesAsync();

            return result;
        }
    }
}

