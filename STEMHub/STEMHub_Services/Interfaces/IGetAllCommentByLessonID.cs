using STEMHub.STEMHub_Data.Entities;

namespace STEMHub.STEMHub_Services.Interfaces
{
    public interface IGetAllCommentByLessonId
    {
        Task<IEnumerable<Comment>> GetAllCommentByLessonID(Guid lessonId);
    }
}
