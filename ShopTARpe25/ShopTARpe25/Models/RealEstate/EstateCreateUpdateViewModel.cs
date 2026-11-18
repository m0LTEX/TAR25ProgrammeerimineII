using ShopTARpe25.Models.Spaceship;

namespace ShopTARpe25.Models.RealEstate
{
    public class EstateCreateUpdateViewModel
    {
        public Guid? Id { get; set; }
        public double? Area { get; set; }
        public string? Address { get; set; }
        public int RoomNumber { get; set; }
        public string? BuildingType { get; set; }

        public List<IFormFile> Files { get; set; }
        public List<EstateImageViewModel> Image { get; set; }
            = new List<EstateImageViewModel>();

        public DateTime? CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
    }
}
