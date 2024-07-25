using System.ComponentModel.DataAnnotations;

namespace STEMHub.STEMHub_Data.Entities
{
    public class Owner
    {
        [Key]
        public Guid Id { get; set; }
        public string? Phone { get; set; }
        public string? Introduction { get; set; }
        public string? Gmail { get; set; }
        public string? Address { get; set; }
    }
}
