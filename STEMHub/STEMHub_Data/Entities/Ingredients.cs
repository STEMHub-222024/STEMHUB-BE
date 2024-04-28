using System.ComponentModel.DataAnnotations.Schema;

namespace STEMHub.STEMHub_Data.Entities
{
    public class Ingredients
    {
        public Guid IngredientsId { get; set; }
        public string? IngredientsName { get; set; }

        public Topic? Topic { get; set; }
        [ForeignKey("TopicId")]
        public Guid TopicId { get; set; }
    }
}
