
using System;

namespace ShopTARpe25.Core.Domain
{
    public class Book
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public int PublishedYear { get; set; }
        public bool IsAvailable { get; set; } = true;
        public string? ExistingFilePath { get; set; }
        public Guid? SpaceshipId { get; set; }
    }
}
