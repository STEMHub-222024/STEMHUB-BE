using System.ComponentModel.DataAnnotations.Schema;

namespace STEMHub.STEMHub_Data.Entities
{
    public class Lesson
    {
        public Guid LessonId { get; set; }
        public string? LessonName { get; set; }
        public string part { get; set; } // nguyên liệu - kết quả - 
        public ICollection<Comment> Comment { get; set; } = new List<Comment>();
        public Topic? Topic { get; set; }
        [ForeignKey("TopicId")]
        public Guid TopicId { get; set; }

        public ICollection<Video> Videos { get; set; } = new List<Video>();
    }
}
