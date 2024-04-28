using System.ComponentModel.DataAnnotations;

namespace STEMHub.STEMHub_Data.DTO
{
    public class BannerDto
    {
        public Guid BannerId { get; set; }
        public string? Image { get; set; }
        public string? Title { get; set; }
        public string? Content { get; set; }
    }
}
