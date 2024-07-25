using System.ComponentModel.DataAnnotations;

namespace STEMHub.STEMHub_Data.DTO
{
    public class OwnerDto
    {
        [Key]
        public Guid Id { get; set; }
        public string? Phone { get; set; }
        public string? Introduction { get; set; }
        public string? Gmail { get; set; }
        public string? Address { get; set; }
    }
}
