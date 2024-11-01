using STEMHub.STEMHub_Data.Data;
using STEMHub.STEMHub_Data.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace STEMHub.STEMHub_Data.DTO
{
    public class LikeDto
    {
        public Guid LikeId { get; set; }
        public Guid NewspaperArticleId { get; set; }
        public string UserId { get; set; }
        public DateTime LikedAt { get; set; } = DateTime.Now;
    }
}
