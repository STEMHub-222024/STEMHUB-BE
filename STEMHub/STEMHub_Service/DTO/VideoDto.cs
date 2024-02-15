namespace STEMHub.STEMHub_Service.DTO
{
    public class VideoDto
    {
        public Guid VideoId { get; set; }
        public string? VideoTitle { get; set; }
        public string? Path { get; set; }
        public Guid LessonId { get; set; }
    }
}
