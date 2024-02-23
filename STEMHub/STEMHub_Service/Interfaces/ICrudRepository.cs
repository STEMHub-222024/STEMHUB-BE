using System.Linq.Expressions;

namespace STEMHub.STEMHub_Service.Interfaces
{
    public interface ICrudRepository<T> where T : class
    {
        Task<IEnumerable<TDto>> GetAllAsync<TDto>();
        Task<TDto> GetByIdAsync<TDto>(Guid id);
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(Guid id);
        Task<IEnumerable<TDto>> SearchAsync<TDto>(Expression<Func<T, bool>> predicate);
    }
}
