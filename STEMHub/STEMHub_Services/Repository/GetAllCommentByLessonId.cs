using AutoMapper;
using Microsoft.EntityFrameworkCore;
using STEMHub.STEMHub_Data.Data;
using STEMHub.STEMHub_Data.Entities;
using STEMHub.STEMHub_Services.Interfaces;

namespace STEMHub.STEMHub_Services.Repository
{
    public class GetAllCommentByLessonId : IGetAllCommentByLessonId
    {
        private readonly STEMHubDbContext _context;
        public GetAllCommentByLessonId(STEMHubDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Comment>> GetAllCommentByLessonID(Guid lessonId)
        {
            var lessonExists = await _context.Set<Lesson>().AnyAsync(l => l.LessonId == lessonId);
            if (!lessonExists)
            {
                throw new KeyNotFoundException($"Không tìm thấy bài học với ID: {lessonId}");
            }

            var comments = await _context.Set<Comment>()
                .Where(c => c.LessonId == lessonId)
                .ToListAsync();

            return comments;
        }
    }
}
