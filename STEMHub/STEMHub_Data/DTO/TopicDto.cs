using System.ComponentModel.DataAnnotations.Schema;

namespace STEMHub.STEMHub_Data.DTO
{
    public class TopicDto
    {
        public Guid TopicId { get; set; }
        public string? TopicName { get; set; }
        public string? TopicImage { get; set; }
        public string? VideoReview { get; set; }
        public int View { get; set; }
        public string? Description { get; set; }
        public Guid STEMId { get; set; }
    }
}
