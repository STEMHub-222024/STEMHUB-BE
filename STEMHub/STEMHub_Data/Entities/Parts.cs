﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace STEMHub.STEMHub_Data.Entities
{
    public class Parts
    {
        [Key]
        public Guid PartId { get; set; }
        public string MaterialsMarkdown { get; set; } = string.Empty;
        public string MaterialsHtmlContent { get; set; } = string.Empty;
        public string StepsMarkdown { get; set; } = string.Empty;
        public string StepsHtmlContent { get; set; } = string.Empty;
        public string ResultsMarkdown { get; set; } = string.Empty;
        public string ResultsHtmlContent { get; set; } = string.Empty;

        [ForeignKey("Lesson")]
        public Guid LessonId { get; set; }
        public Lesson Lesson { get; set; }
    }
}
