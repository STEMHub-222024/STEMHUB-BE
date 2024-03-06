using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using STEMHub.STEMHub_Data.Data;

namespace STEMHub.STEMHub_Data.Entities
{
    public class NewspaperArticle
    {
        [Key]
        public Guid NewspaperArticleId { get; set; }
        public string? Title { get; set; }
        public string? Markdown { get; set; }

        [DataType(DataType.MultilineText)]
        public string? HtmlContent { get; set; }


        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}
