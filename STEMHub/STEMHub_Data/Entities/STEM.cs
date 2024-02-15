namespace STEMHub.STEMHub_Data.Entities
{
    public class STEM
    {
        public Guid STEMId { get; set; }
        public string? STEMName { get; set; }

        public ICollection<Topic> Topic { get; set; } = new List<Topic>();
    }
}
