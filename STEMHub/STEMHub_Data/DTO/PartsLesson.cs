namespace STEMHub.STEMHub_Data.DTO
{
    public class PartsLesson
    {
        public Guid LessonId { get; set; }
        public string? LessonName { get; set; }
        public Guid TopicId { get; set; }
        public PartsDto Parts { get; set; }
    }
}
