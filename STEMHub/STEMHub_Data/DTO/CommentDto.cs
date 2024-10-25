using STEMHub.STEMHub_Data.Data;
using System.ComponentModel.DataAnnotations.Schema;
using STEMHub.STEMHub_Data.Entities;

namespace STEMHub.STEMHub_Data.DTO
{
    public class CommentDto
    {
        public Guid CommentId { get; set; }
        public string? Content_C { get; set; }
        public int Rate { get; set; }
        public CommentType Type { get; set; }
        public Guid? NewspaperArticleId { get; set; }
        public Guid? LessonId { get; set;}
        public string? UserId { get; set; }
    }
}
