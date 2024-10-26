namespace STEMHub.STEMHub_Data.Entities
{
    public class QuestionSearch
    {
        public Guid QuestionSearchId { get; set; }
        public Guid QuestionId { get; set; }
        public DateTime SearchedDate { get; set; } = DateTime.UtcNow;

        public Question Question { get; set; }
    }
}
