using STEMHub.STEMHub_Data.Data;
using System.ComponentModel.DataAnnotations.Schema;

namespace STEMHub.STEMHub_Data.Entities
{
    public class Comment
    {
        public Guid CommentId { get; set; }
        public string? Content_C { get; set; }
        public int Rate { get; set; }

        public Lesson? Lesson { get; set; }
        [ForeignKey("LessonId")]
        public Guid LessonId { get; set;}

        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}
