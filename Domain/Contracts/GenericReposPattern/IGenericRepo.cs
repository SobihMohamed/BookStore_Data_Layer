using Domain.Contracts.SpecificationPattern;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Contracts.GenericReposPattern
{
    public interface IGenericRepo<TEntity, TKey> where TEntity : IEntity<TKey>
    {
        Task<IReadOnlyList<TEntity>> GetAllAsync();
        Task<TEntity?> GetByIdAsync(TKey id);
        Task<int> CountAsync();
        Task AddAsync(TEntity entity);
        void Update(TEntity entity);
        void Delete(TEntity entity);

        Task<IReadOnlyList<TEntity>> GetAllWithSpecAsync(ISpecifications<TEntity, TKey> specifications);
        Task<TEntity?> GetByIdWithSpecAsync(ISpecifications<TEntity, TKey> specifications);
        Task<int> GetCountAsync(ISpecifications<TEntity, TKey> specifications);
    }
}