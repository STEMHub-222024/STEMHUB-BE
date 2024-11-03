using STEMHub.STEMHub_Data.DTO;
using STEMHub.STEMHub_Data.Entities;

namespace STEMHub.STEMHub_Services.Interfaces
{
    public interface ILessonRepository : ICrudRepository<Lesson>
    {
        Task<PartsLesson?> GetLessonWithPartsAsync(Guid id);
    }

}
