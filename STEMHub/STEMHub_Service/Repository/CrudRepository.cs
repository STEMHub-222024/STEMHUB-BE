using AutoMapper;
using STEMHub.STEMHub_Data.Data;
using STEMHub.STEMHub_Service.Interfaces;
using System.Linq.Expressions;

namespace STEMHub.STEMHub_Service.Repository
{
    public class CrudRepository<T> : ICrudRepository<T> where T : class
    {
        private readonly STEMHubDbContext _context;
        private readonly IMapper _mapper;

        public CrudRepository(STEMHubDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public TDto GetById<TDto>(Guid id)
        {
            var entity = _context.Set<T>().Find(id);
            if (entity != null)
            {
                return MapToDto<TDto>(entity);
            }

            return default!;
        }

        public IEnumerable<TDto> GetAll<TDto>()
        {
            var entities = _context.Set<T>().ToList();
            // ReSharper disable once ConvertClosureToMethodGroup
            return entities.Select(entity => MapToDto<TDto>(entity));
        }

        public async Task Add(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
        }

        public Task Update(T entity)
        {
            _context.Set<T>().Update(entity);
            return Task.CompletedTask;
        }

        public async Task Delete(Guid id)
        {
            var entity = await _context.Set<T>().FindAsync(id);
            if (entity != null)
            {
                _context.Set<T>().Remove(entity);
                
            }
        }

        private TDto MapToDto<TDto>(T entity)
        {
            return _mapper.Map<TDto>(entity)!;
        }

        public IEnumerable<TDto> Search<TDto>(Expression<Func<T, bool>> predicate)
        {
            var entities = _context.Set<T>().Where(predicate).ToList();
            // ReSharper disable once ConvertClosureToMethodGroup
            return entities.Select(entity => MapToDto<TDto>(entity));
        }
    }
}
