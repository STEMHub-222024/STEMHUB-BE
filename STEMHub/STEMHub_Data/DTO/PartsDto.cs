using System.ComponentModel.DataAnnotations;

namespace STEMHub.STEMHub_Data.DTO
{
    public class PartsDto
    {
        public Guid PartId { get; set; }
        public string MaterialsMarkdown { get; set; } = string.Empty;
        public string MaterialsHtmlContent { get; set; } = string.Empty;
        public string StepsMarkdown { get; set; } = string.Empty;
        public string StepsHtmlContent { get; set; } = string.Empty;
        public string ResultsMarkdown { get; set; } = string.Empty;
        public string ResultsHtmlContent { get; set; } = string.Empty;
        public Guid LessonId { get; set; }
    }
}
