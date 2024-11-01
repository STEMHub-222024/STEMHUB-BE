using System.ComponentModel.DataAnnotations;

namespace STEMHub.STEMHub_Data.Entities
{
    public class Search
    {
        [Key]
        public Guid SearchId { get; set; }
        public string SearchKeyword { get; set; } = string.Empty;
        public int SearchCount { get; set; }
    }
}
