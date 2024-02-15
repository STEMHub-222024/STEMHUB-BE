using System.ComponentModel.DataAnnotations.Schema;
using STEMHub.STEMHub_Data.Entities;

namespace STEMHub.STEMHub_Service.DTO
{
    public class LessonDto
    {
        public Guid LessonId { get; set; }
        public string? LessonName { get; set; }
        public Guid TopicId { get; set; }
    }
}
