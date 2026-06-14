using Domain.Contracts;
using Domain.Contracts.GenericReposPattern;
using Domain.Contracts.SpecificationPattern;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;
using Persistence.Evaluator;

namespace Persistence.Implementpatterns.RepoImplement
{
    public class GenericRepo<TEntity, TKey>
       : IGenericRepo<TEntity, TKey> where TEntity : class, IEntity<TKey>
    {
        private readonly DbSet<TEntity> _dbSet;

        public GenericRepo(BookDbContext context)
        {
            _dbSet = context.Set<TEntity>();
        }

        public async Task<IReadOnlyList<TEntity>> GetAllAsync()
            => await _dbSet.AsNoTracking().ToListAsync();

        public async Task<TEntity?> GetByIdAsync(TKey id)
            => await _dbSet.FindAsync(id);

        public async Task AddAsync(TEntity entity)
            => await _dbSet.AddAsync(entity);

        public void Update(TEntity entity)
            => _dbSet.Update(entity);

        public void Delete(TEntity entity)
            => _dbSet.Remove(entity);

        public async Task<IReadOnlyList<TEntity>> GetAllWithSpecAsync(ISpecifications<TEntity, TKey> specifications)
        {
            var BaseQuery = _dbSet.AsQueryable();
            var Query = SpecificationEvaluator.GenerateQuery(BaseQuery, specifications);
            return await Query.ToListAsync();
        }

        public async Task<TEntity?> GetByIdWithSpecAsync(ISpecifications<TEntity, TKey> specifications)
        {
            var BaseQuery = _dbSet.AsQueryable();
            var Query = SpecificationEvaluator.GenerateQuery(BaseQuery, specifications);
            return await Query.FirstOrDefaultAsync();
        }

        public async Task<int> GetCountAsync(ISpecifications<TEntity, TKey> specifications)
        {
            var BaseQuery = _dbSet.AsQueryable();
            var Query = SpecificationEvaluator.GenerateQuery(BaseQuery, specifications);
            return await Query.CountAsync();
        }

        public async Task<int> CountAsync()
            => await _dbSet.CountAsync();
    }
}