using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopTARpe25.Core.Dto;
using ShopTARpe25.Core.ServiceInterface;
using ShopTARpe25.Data;
using ShopTARpe25.Models.RealEstate;


namespace ShopTARpe25.Controllers
{
        public class RealEstateController : Controller
        {
            private readonly IRealEstateServices _realestateService;
            private readonly ShopTARpe25Context _context;
            private readonly IFileServices _fileService;
            //teha constructor et saaks kasutada teenust, mis on
            //defineeritud IRealEstateServices liideses
            //lisage Context
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


            public IActionResult Index()
            {
                var result = _context.Realestates
                    .Select(x => new EstateIndexViewModel
                    {
                        Id = x.Id,
                        Area = x.Area,
                        Address = x.Address,
                        RoomNumber = x.RoomNumber,
                        BuildingType = x.BuildingType
                    })
                    .ToList();

                return View("~/Views/RealEstate/Index.cshtml", result);
            }

            //kui kasutaja klikib "Create" nuppu, siis see meetod käivitatakse
            //tagastab kasutajale vormi, kuhu saab sisestada andmed
            [HttpGet]
            public IActionResult Create()
            {
                EstateCreateUpdateViewModel vm = new();

                return View("CreateUpdate", vm);
            }

            //kui oled teinud vormi, siis see meetod käivitatakse
            //saadab andmed serverisse, kus need salvestatakse andmebaasi
            [HttpPost]
            public async Task<IActionResult> Create(EstateCreateUpdateViewModel vm)
            {
                //luua vaheinstants, mis sisaldab andmeid, mis on saadud vormist
                //need andmed tuleb edasi saata dto-sse, mis on mõeldud andmebaasi salvestamiseks

                var dto = new RealEstateDto
                {
                    Id = vm.Id,
                    Area = vm.Area,
                    Address = vm.Address,
                    RoomNumber = vm.RoomNumber,
                    BuildingType = vm.BuildingType,
                    Files = vm.Files,
                    FileToApiDtos = vm.Image
                        .Select(file => new FileToApiDto
                        {
                            Id = file.ImageId,
                            ExistingFilePath = file.FilePath,
                            RealEstateId = file.RealEstateId
                        }).ToArray()
                };

                //kutsuda teenuse meetodit, mis salvestab andmed andmebaasi
                var result = await _realestateService.Create(dto);

                if (result == null)
                {
                    return RedirectToAction(nameof(Index));
                }

                return RedirectToAction(nameof(Index));
            }

            //tuleb teha Details meetod
            //see kutsub v'lja interfacest service meetodi

            [HttpGet]
            public async Task<IActionResult> Details(Guid id)
            {
                //meetodi kutsumine interfacest
                var realEstate = await _realestateService.DetailsAsync(id);

                //veakäsitlus
                //suunab vaatele NotFound, kui andmeid ei ole
                if (realEstate == null)
                {
                    return NotFound();
                }

            var images = await _context.FileToApis
                .Where(x => x.RealEstateId == id)
                .Select(y => new EstateImageViewModel
                {
                    FilePath = /*"~/multipleFileUpload/" +*/ y.ExistingFilePath,
                    ImageId = y.Id,
                    RealEstateId = y.RealEstateId
                }).ToArrayAsync();

                //tuleb teha viewModel ja see siin välja kutsuda
                //ära map-ida vm ja domain
                var vm = new EstateDetailsViewModel();

                vm.Id = realEstate.Id;
                vm.Area = realEstate.Area;
                vm.Address = realEstate.Address;
                vm.RoomNumber = realEstate.RoomNumber;
                vm.BuildingType = realEstate.BuildingType;
                vm.CreatedAt = realEstate.CreatedAt;
                vm.ModifiedAt = realEstate.ModifiedAt;
                vm.Images.AddRange(images);

                return View(vm);
            }

            [HttpGet]
            public async Task<IActionResult> Update(Guid id)
            {
                var realEstate = await _realestateService.DetailsAsync(id);

                if (realEstate == null)
                {
                    return NotFound();
                }

                var images = await _context.FileToApis
                    .Where(x => x.RealEstateId == id)
                    .Select(y => new EstateImageViewModel
                    {
                        FilePath = y.ExistingFilePath,
                        ImageId = y.Id,
                        RealEstateId = y.RealEstateId
                    }).ToArrayAsync();

                var vm = new EstateCreateUpdateViewModel();

                vm.Id = realEstate.Id;
                vm.Area = realEstate.Area;
                vm.Address = realEstate.Address;
                vm.RoomNumber = realEstate.RoomNumber;
                vm.BuildingType = realEstate.BuildingType;
                vm.CreatedAt = realEstate.CreatedAt;
                vm.ModifiedAt = realEstate.ModifiedAt;
                vm.Image.AddRange(images);

                return View("CreateUpdate", vm);
            }

            [HttpPost]
            public async Task<IActionResult> Update(EstateUpdateViewModel vm)
            {
                var dto = new RealEstateDto
                {
                    Id = vm.Id,
                    Area = vm.Area,
                    Address = vm.Address,
                    RoomNumber = vm.RoomNumber,
                    BuildingType = vm.BuildingType,
                    Files = vm.Files,
                    FileToApiDtos = vm.Images
                        .Select(file => new FileToApiDto
                        {
                            Id = file.ImageId,
                            ExistingFilePath = file.FilePath,
                            RealEstateId = file.RealEstateId
                        }).ToArray()
                };

                var result = await _realestateService.Update(dto);

                if (result == null)
                {
                    return RedirectToAction(nameof(Index));
                }

                return RedirectToAction(nameof(Index));
            }

            [HttpGet]
            public async Task<IActionResult> Delete(Guid id)
            {
                var realEstate = await _realestateService.DetailsAsync(id);

                if (realEstate == null)
                {
                    return NotFound();
                }

                var images = await _context.FileToApis
                    .Where(x => x.RealEstateId == id)
                    .Select(y => new EstateImageViewModel
                    {
                        FilePath = y.ExistingFilePath,
                        ImageId = y.Id,
                        RealEstateId = y.RealEstateId
                    }).ToArrayAsync();
                //kirjuta kood, mis aitab näidata pilte, mis 
                //kuuluvad konkreetsele kosmoselaevale
                //kui hakkan kustutama, siis saan teada, mida ma kustutan 

                var vm = new EstateDeleteViewModel();

                vm.Id = realEstate.Id;
                vm.Area = realEstate.Area;
                vm.Address = realEstate.Address;
                vm.RoomNumber = realEstate.RoomNumber;
                vm.BuildingType = realEstate.BuildingType;
                vm.CreatedAt = realEstate.CreatedAt;
                vm.ModifiedAt = realEstate.ModifiedAt;
                vm.Images.AddRange(images);

                return View(vm);
            }

            [HttpPost]
            public async Task<IActionResult> DeleteConfirmation(Guid id)
            {
                var result = await _realestateService.Delete(id);

                if (result == null)
                {
                    return RedirectToAction(nameof(Index));
                }

                return RedirectToAction(nameof(Index));
            }

            [HttpPost]
            public async Task<IActionResult> RemoveImage(EstateImageViewModel vm)
            {
                //tuleb ühendada dto ja viewModel, et saaks kasutada teenuse meetodit
                var dto = new FileToApiDto()
                {
                    Id = vm.ImageId
                };

                //kutsuda teenuse meetodit, mis kustutab pildi andmebaasist
                var image = await _fileService.RemoveImageFromApi(dto);

                if (image == null)
                {
                    return RedirectToAction(nameof(Index));
                }

                return RedirectToAction(nameof(Index));
            }
        }   
}
