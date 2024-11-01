using AutoMapper;
using Microsoft.EntityFrameworkCore;
using STEMHub.STEMHub_Data.Data;
using STEMHub.STEMHub_Data.Entities;

namespace STEMHub.STEMHub_Services.Repository
{
    public class CommentRepository : CrudRepository<Comment>
    {
        public CommentRepository(STEMHubDbContext context, IMapper mapper) : base(context, mapper) { }

        public async Task<IEnumerable<TDto>> GetCommentsByTypeAndIdAsync<TDto>(CommentType type, Guid? lessonId, Guid? articleId)
        {
            var query = _context.Set<Comment>().AsQueryable();

            // Lọc theo loại
            query = query.Where(c => c.Type == type);

            // Lọc theo lessonId nếu type là Lesson
            if (type == CommentType.Lesson)
            {
                if (lessonId.HasValue)
                {
                    query = query.Where(c => c.LessonId == lessonId.Value);
                }
                else
                {
                    // Nếu không có lessonId, trả về không có kết quả
                    return Enumerable.Empty<TDto>();
                }
            }
            // Lọc theo articleId nếu type là Newspaper
            else if (type == CommentType.Newspaper)
            {
                if (articleId.HasValue)
                {
                    query = query.Where(c => c.NewspaperArticleId == articleId.Value);
                }
                else
                {
                    // Nếu không có articleId, trả về không có kết quả
                    return Enumerable.Empty<TDto>();
                }
            }

            // Thực hiện truy vấn và chuyển đổi kết quả thành DTO
            var entities = await query.ToListAsync();
            return entities.Select(entity => MapToDto<TDto>(entity));
        }

    }
}
