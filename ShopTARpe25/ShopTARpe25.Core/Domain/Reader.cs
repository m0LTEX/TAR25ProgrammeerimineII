
using System;

namespace ShopTARpe25.Core.Domain
{
    public class Reader
    {
        public Guid Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public DateTime? BuiltDate { get; set; }
        public string? Classification { get; set; }
        public int? Crew { get; set; }
        public int? EnginePower { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
    }
}
