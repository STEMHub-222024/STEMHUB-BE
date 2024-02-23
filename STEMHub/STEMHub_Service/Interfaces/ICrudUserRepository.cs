using System.Linq.Expressions;

namespace STEMHub.STEMHub_Service.Interfaces
{
    public interface ICrudUserRepository<T> where T : class
    {
        Task<TDto> GetByIdUserAsync<TDto>(string id);
        Task UpdateUserAsync(T entity);
        Task DeleteUserAsync(string id);
    }
}
