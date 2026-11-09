
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopTARpe25.Core.Dto;
using ShopTARpe25.Core.ServiceInterface;
using ShopTARpe25.Data;
using ShopTARpe25.Models.RealEstate;
using ShopTARpe25.Models.Spaceship;

namespace ShopTARpe25.Controllers
{
    public class RealEstateController : Controller
    {
        private readonly IRealEstateServices _realestateService;
        private readonly ShopTARpe25Context _context;
        private readonly IFileServices _fileService;

        public RealEstateController
        (
            IRealEstateServices realestateService,
            ShopTARpe25Context context,
            IFileServices fileService
        )
        {
            _realestateService = realestateService;
            _context = context;
            _fileService = fileService;
        }

        // INDEX
        public IActionResult Index()
        {
            var result = _context.Realestates
                .Select(x => new RealEstateIndexViewModel
                {
                    Id = x.Id,
                    Area = x.Area,
                    Address = x.Address,
                    RoomNumber = x.RoomNumber,
                    BuildingType = x.BuildingType
                })
                .ToList();

            return View(result);
        }

        // CREATE GET
        [HttpGet]
        public IActionResult Create()
        {
            RealEstateCreateUpdateViewModel vm = new();

            return View("CreateUpdate", vm);
        }

        // CREATE POST
        [HttpPost]
        public async Task<IActionResult> Create(RealEstateCreateUpdateViewModel vm)
        {
            var dto = new RealEstateDto
            {
                Id = vm.Id,
                Area = vm.Area,
                Address = vm.Address,
                RoomNumber = vm.RoomNumber,
                BuildingType = vm.BuildingType,
                Files = vm.Files
            };

            await _realestateService.Create(dto);

            return RedirectToAction(nameof(Index));
        }

        // DETAILS
        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            var realEstate = await _realestateService.DetailAsync(id);

            if (realEstate == null)
            {
                return NotFound();
            }

            var images = await _context.FileToApis
                .Where(x => x.RealEstateId.HasValue &&
                            x.RealEstateId.Value == id)
                .Select(y => new ImageViewModel
                {
                    FilePath = y.ExistingFilePath,
                    ImageId = y.Id,
                    RealEstateId = y.RealEstateId
                })
                .ToArrayAsync();

            var vm = new RealEstateDetailsViewModel
            {
                Id = realEstate.Id,
                Area = realEstate.Area,
                Address = realEstate.Address,
                RoomNumber = realEstate.RoomNumber,
                BuildingType = realEstate.BuildingType,
                CreatedAt = realEstate.CreatedAt,
                ModifiedAt = realEstate.ModifiedAt
            };

            vm.Images.AddRange(images);

            return View(vm);
        }

        // UPDATE GET
        [HttpGet]
        public async Task<IActionResult> Update(Guid id)
        {
            var realEstate = await _realestateService.DetailAsync(id);

            if (realEstate == null)
            {
                return NotFound();
            }

            var images = await _context.FileToApis
                .Where(x => x.RealEstateId.HasValue &&
                            x.RealEstateId.Value == id)
                .Select(y => new ImageViewModel
                {
                    FilePath = y.ExistingFilePath,
                    ImageId = y.Id,
                    RealEstateId = y.RealEstateId
                })
                .ToArrayAsync();

            var vm = new RealEstateCreateUpdateViewModel
            {
                Id = realEstate.Id,
                Area = realEstate.Area,
                Address = realEstate.Address,
                RoomNumber = realEstate.RoomNumber,
                BuildingType = realEstate.BuildingType,
                CreatedAt = realEstate.CreatedAt,
                ModifiedAt = realEstate.ModifiedAt
            };

            vm.Images.AddRange(images);

            return View("CreateUpdate", vm);
        }

        // UPDATE POST
        [HttpPost]
        public async Task<IActionResult> Update(RealEstateCreateUpdateViewModel vm)
        {
            var dto = new RealEstateDto
            {
                Id = vm.Id,
                Area = vm.Area,
                Address = vm.Address,
                RoomNumber = vm.RoomNumber,
                BuildingType = vm.BuildingType,
                Files = vm.Files
            };

            await _realestateService.Update(dto);

            return RedirectToAction(nameof(Index));
        }

        // DELETE GET
        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            var realEstate = await _realestateService.DetailAsync(id);

            if (realEstate == null)
            {
                return NotFound();
            }

            var images = await _context.FileToApis
                .Where(x => x.RealEstateId.HasValue &&
                            x.RealEstateId.Value == id)
                .Select(y => new ImageViewModel
                {
                    FilePath = y.ExistingFilePath,
                    ImageId = y.Id,
                    RealEstateId = y.RealEstateId
                })
                .ToArrayAsync();

            var vm = new RealEstateDeleteViewModel
            {
                Id = realEstate.Id,
                Area = realEstate.Area,
                Address = realEstate.Address,
                RoomNumber = realEstate.RoomNumber,
                BuildingType = realEstate.BuildingType,
                CreatedAt = realEstate.CreatedAt,
                ModifiedAt = realEstate.ModifiedAt
            };

            vm.Images.AddRange(images);

            return View(vm);
        }

        
        [HttpPost]
        public async Task<IActionResult> DeleteConfirmation(Guid id)
        {
            await _realestateService.Delete(id);

            return RedirectToAction(nameof(Index));
        }

        // REMOVE IMAGE
        [HttpPost]
        public async Task<IActionResult> RemoveImage(ImageViewModel vm)
        {
            var dto = new FileToApiDto
            {
                Id = vm.ImageId
            };

            await _fileService.RemoveImageFromApi(dto);

            return RedirectToAction(nameof(Index));
        }
    }
}
