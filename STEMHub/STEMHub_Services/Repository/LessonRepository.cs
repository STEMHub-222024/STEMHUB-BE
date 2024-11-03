using AutoMapper;
using Microsoft.EntityFrameworkCore;
using STEMHub.STEMHub_Data.Data;
using STEMHub.STEMHub_Data.DTO;
using STEMHub.STEMHub_Data.Entities;
using STEMHub.STEMHub_Services.Interfaces;

namespace STEMHub.STEMHub_Services.Repository
{
    public class LessonRepository : CrudRepository<Lesson>, ILessonRepository
    {
        public LessonRepository(STEMHubDbContext context, IMapper mapper)
            : base(context, mapper)
        {
        }

        public async Task<PartsLesson?> GetLessonWithPartsAsync(Guid id)
        {
            var lesson = await _context.Lesson
                .Include(l => l.Parts)
                .FirstOrDefaultAsync(l => l.LessonId == id);

            if (lesson == null) return null;

            return new PartsLesson
            {
                LessonId = lesson.LessonId,
                LessonName = lesson.LessonName,
                TopicId = lesson.TopicId,
                Parts = lesson.Parts != null ? new PartsDto
                {
                    PartId = lesson.Parts.PartId,
                    MaterialsMarkdown = lesson.Parts.MaterialsMarkdown,
                    MaterialsHtmlContent = lesson.Parts.MaterialsHtmlContent,
                    StepsMarkdown = lesson.Parts.StepsMarkdown,
                    StepsHtmlContent = lesson.Parts.StepsHtmlContent,
                    ResultsMarkdown = lesson.Parts.ResultsMarkdown,
                    ResultsHtmlContent = lesson.Parts.ResultsHtmlContent,
                    LessonId = lesson.Parts.LessonId
                } : null
            };
        }
    }

}
