using AutoMapper;
using Microsoft.EntityFrameworkCore;
using STEMHub.STEMHub_Data.Data;
using STEMHub.STEMHub_Data.Entities;
using System.Linq.Expressions;

namespace STEMHub.STEMHub_Services.Repository
{
    public class CommentRepository : CrudRepository<Comment>
    {
        public CommentRepository(STEMHubDbContext context, IMapper mapper) : base(context, mapper) { }

        public async Task<IEnumerable<TDto>> GetCommentsByTypeAndIdAsync<TDto>(CommentType type, Guid? lessonId, Guid? articleId)
        {
            var query = _context.Set<Comment>().AsQueryable();

            query = query.Where(c => c.Type == type);

            if (type == CommentType.Lesson)
            {
                if (lessonId.HasValue)
                {
                    query = query.Where(c => c.LessonId == lessonId.Value);
                }
                else
                {
                    return Enumerable.Empty<TDto>();
                }
            }
            else if (type == CommentType.Newspaper)
            {
                if (articleId.HasValue)
                {
                    query = query.Where(c => c.NewspaperArticleId == articleId.Value);
                }
                else
                {
                    return Enumerable.Empty<TDto>();
                }
            }
            var entities = await query.ToListAsync();
            return entities.Select(entity => MapToDto<TDto>(entity));
        }

        public async Task<int> CountAsync(Expression<Func<Comment, bool>> predicate)
        {
            return await _context.Comment.CountAsync(predicate);
        }

    }
}
