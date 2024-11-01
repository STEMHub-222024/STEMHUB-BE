using STEMHub.STEMHub_Data.DTO;

namespace STEMHub.STEMHub_Services.Interfaces
{
    public interface ISearchService
    {
        Task UpdateSearchKeywordAsync(string lessonKey);
        Task<IEnumerable<SearchKeywordsDto>> GetTopKeywordsAsync(int top = 5);
    }
}
