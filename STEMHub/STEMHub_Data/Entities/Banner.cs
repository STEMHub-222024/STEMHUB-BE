using System.ComponentModel.DataAnnotations;

namespace STEMHub.STEMHub_Data.Entities
{
    public class Banner
    {
        [Key]
        public Guid BannerId { get; set; }
        public string? Image { get; set; }
        public string? Title { get; set; }
        public string? Content { get; set; }
    }
}
