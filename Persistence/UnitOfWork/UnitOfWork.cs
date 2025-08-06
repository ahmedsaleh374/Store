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
    public class UnitOfWork : IUnitOfWork
    {
        private readonly StoreDbContext _context;
        private ConcurrentDictionary<string,object> _repositories;

        public UnitOfWork(StoreDbContext context)
        {
            _context = context;
            //_repositories = new ConcurrentDictionary<string,object>();
            _repositories = new ();
        }
        public IGenericRepository<TEntity, TKey> GetRepository<TEntity, TKey>() where TEntity : BaseEntity<TKey>
         => (IGenericRepository<TEntity, TKey>)_repositories.GetOrAdd(typeof(TEntity).Name, _ => new GenericRepository<TEntity, TKey>(_context));

        public async Task SaveChangesAsync()
         => await _context.SaveChangesAsync();
    }
}
