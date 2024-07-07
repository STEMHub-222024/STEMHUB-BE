
namespace STEMHub.STEMHub_Data.Entities
{
    public class Video
    {
        public Guid VideoId { get; set; }
        public string? VideoTitle { get; set; }
        public string? Description_V { get; set; }
        public string? Path { get; set; }

        public Guid LessonId { get; set; }
        public Lesson Lesson { get; set; } = null!;
    }
}
