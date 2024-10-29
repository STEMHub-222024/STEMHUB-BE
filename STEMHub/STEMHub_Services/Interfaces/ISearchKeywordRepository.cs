using STEMHub.STEMHub_Data.Entities;

namespace STEMHub.STEMHub_Services.Interfaces
{
    public interface ISearchKeywordRepository
    {
        Task<Search> GetByKeywordAsync(string keyword);
        Task AddAsync(Search keyword);
        Task UpdateAsync(Search keyword);
        Task<IEnumerable<Search>> GetTopKeywordsAsync(int top);
    }
}
