﻿namespace STEMHub.STEMHub_Data.DTO
{
    public class ScientistDto
    {
        public Guid ScientistId { get; set; }
        public string? FullName { get; set; }
        public string? Image { get; set; }
        // Châm ngôn
        public string? Adage { get; set; }
        public string? AdageVN { get; set; }
        public string? DescriptionScientist { get; set; }
        public string? ContentMarkdown { get; set; }
        public string? Content { get; set; }
    }
}
