using Domain.Contracts;
using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Persistence.Data;
using Persistence.Repositories;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.UnitOfWork
{
    public class UnitOfWork(StoreDbContext context) : IUnitOfWork
    {
        
        private ConcurrentDictionary<string,object> _repositories;

        public IGenericRepository<TEntity, TKey> GenericRepository<TEntity, TKey>() where TEntity : BaseEntity<TKey>
         => (IGenericRepository<TEntity, TKey>)_repositories.GetOrAdd(typeof(TEntity).Name, _ => new GenericRepository<TEntity, TKey>(context));

        public async Task SaveChangesAsync()
         => await context.SaveChangesAsync();
    }
}
