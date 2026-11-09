using Microsoft.AspNetCore.Http;

namespace ShopTARpe25.Core.Dto
{
    public class RealEstateDto
    {
        public Guid? Id { get; set; }
        public double? Area { get; set; }
        public string? Address { get; set; }
        public int RoomNumber { get; set; }
        public string? BuildingType { get; set; }

        public DateTime? CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }

        public IFormFile[]? Files { get; set; }
        public FileToApiDto[]? FileToApiDtos { get; set; }
    }
}