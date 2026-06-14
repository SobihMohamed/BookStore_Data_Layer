using Domain.Contracts.GenericReposPattern;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Contracts.UnitOfWorkPattern
{
    public interface IUnitOfWork : IAsyncDisposable
    // IAsyncDisposable is used to close the database connection when
    // the unit of work is disposed, ensuring that resources are released properly.
    {
        IGenericRepo<TEntity, TKey> GetRepository<TEntity, TKey>() where TEntity : class, IEntity<TKey>;
        Task<int> SaveChangesAsync();
    }
}
