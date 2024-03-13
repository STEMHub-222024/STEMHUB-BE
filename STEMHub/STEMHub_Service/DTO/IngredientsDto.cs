using STEMHub.STEMHub_Data.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace STEMHub.STEMHub_Service.DTO
{
    public class IngredientsDto
    {
        public Guid IngredientsId { get; set; }
        public string? IngredientsName { get; set; }
        public Guid TopicId { get; set; }
    }
}
