using System.ComponentModel.DataAnnotations.Schema;

namespace STEMHub.STEMHub_Data.Entities
{
    public class Topic
    {
        public Guid TopicId { get; set; }
        public string? TopicName { get; set; }
        public string? TopicImage { get; set; }
        public int View { get; set; }

        public STEM STEM { get; set; }
        [ForeignKey("STEMId")]
        public Guid STEMId { get; set; }

        public ICollection<Lesson> Lessons { get; set; } = new List<Lesson>();
    }
}
