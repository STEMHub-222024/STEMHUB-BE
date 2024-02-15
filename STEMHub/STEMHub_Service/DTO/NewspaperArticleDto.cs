using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using STEMHub.STEMHub_Data.Data;

namespace STEMHub.STEMHub_Service.DTO
{
    public class NewspaperArticleDto
    {
        [Key]
        public Guid NewspaperArticleId { get; set; }
        public string? Title { get; set; }
        public string? Content_NA { get; set; }
        public string? Image { get; set; }
        public string? UserId { get; set; }
    }
}
