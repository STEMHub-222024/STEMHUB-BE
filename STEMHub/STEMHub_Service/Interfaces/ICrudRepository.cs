using System.Linq.Expressions;

namespace STEMHub.STEMHub_Service.Interfaces
{
    public interface ICrudRepository<T> where T : class
    {
        IEnumerable<TDto> GetAll<TDto>();
        public TDto GetById<TDto>(Guid id);
        public Task Add(T entity);
        public Task Update(T entity);
        public Task Delete(Guid id);
        IEnumerable<TDto> Search<TDto>(Expression<Func<T, bool>> predicate);
    }
}
