namespace STEMHub.STEMHub_Data.Entities
{
    public class Scientist
    {
        public Guid ScientistId { get; set; }
        public string? FullName { get; set; }
        public string? Image { get; set; }
        // Châm ngôn
        public string? Adage { get; set; }
        public string? Content { get; set; }
    }
}
