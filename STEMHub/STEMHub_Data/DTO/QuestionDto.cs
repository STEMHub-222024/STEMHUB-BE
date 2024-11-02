namespace STEMHub.STEMHub_Data.DTO
{
    public class QuestionDto
    {
        public Guid QuestionId { get; set; }
        public string Content { get; set; }
        public string Answer { get; set; }
        public DateTime CreatedDate { get; set; }
        public int SearchCount { get; set; }
    }
}
