namespace STEMHub.STEMHub_Data.Entities
{
    public class Question
    {
        public Guid QuestionId { get; set; }
        public string Content { get; set; }
        public string Answer { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public bool IsActive { get; set; } = true;

        public ICollection<QuestionSearch> QuestionSearches { get; set; }
    }
}
