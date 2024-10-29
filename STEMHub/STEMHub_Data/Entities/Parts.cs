using System.ComponentModel.DataAnnotations;

namespace STEMHub.STEMHub_Data.Entities
{
    public class Parts
    {
        [Key]
        public Guid PartId { get; set; }
        public string PartName { get; set; } = string.Empty;
    }
}
