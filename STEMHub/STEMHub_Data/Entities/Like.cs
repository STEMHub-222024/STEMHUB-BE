using STEMHub.STEMHub_Data.Data;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace STEMHub.STEMHub_Data.Entities
{
    public class Like
    {
        [Key]
        public Guid LikeId { get; set; }

        public Guid NewspaperArticleId { get; set; }
        [ForeignKey("NewspaperArticleId")]
        public virtual NewspaperArticle NewspaperArticle { get; set; }

        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual ApplicationUser ApplicationUser { get; set; }

        public DateTime LikedAt { get; set; } = DateTime.Now;
    }
}
