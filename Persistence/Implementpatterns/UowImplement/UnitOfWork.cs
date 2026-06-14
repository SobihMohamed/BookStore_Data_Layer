using Domain.Contracts;
using Domain.Contracts.GenericReposPattern;
using Domain.Contracts.UnitOfWorkPattern;
using Persistence.Context;
using Persistence.Implementpatterns.RepoImplement;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Persistence.Implementpatterns.UowImplement
{
    public class UnitOfWork(BookDbContext context) : IUnitOfWork
    {
        private readonly BookDbContext _context = context;
        private readonly Dictionary<string, object> _repositories = new();

        public IGenericRepo<TEntity, TKey> GetRepository<TEntity, TKey>() where TEntity : class, IEntity<TKey>
        {
            var type = typeof(TEntity).Name;

            if (_repositories.ContainsKey(type))
                return (IGenericRepo<TEntity, TKey>)_repositories[type];

            var repositoryInstance = new GenericRepo<TEntity, TKey>(_context);

            _repositories.Add(type, repositoryInstance);

            return repositoryInstance;
        }

        public async Task<int> SaveChangesAsync()
            => await _context.SaveChangesAsync();

        public async ValueTask DisposeAsync()
            => await _context.DisposeAsync();
    }
}