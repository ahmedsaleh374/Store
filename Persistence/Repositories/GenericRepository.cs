using Domain.Contracts;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    public class GenericRepository<TEntity, Tkey>(StoreDbContext context) : IGenericRepository<TEntity, Tkey>
        where TEntity : BaseEntity<Tkey>
    {
        #region Old Way for DI 
        //private readonly StoreDbContext _context;

        //public GenericRepository(StoreDbContext context)
        //{
        //    _context = context;
        //} 
        #endregion


        public async Task AddAsync(TEntity entity)
             => await context.AddAsync(entity);

        public void Delete(TEntity entity)
             => context.Set<TEntity>().Remove(entity);

        public void Update(TEntity entity)
             => context.Set<TEntity>().Update(entity);

        public async Task<IEnumerable<TEntity>> GetAllAsync(bool isTrackable = false)
        {
            if (isTrackable)
                return await context.Set<TEntity>().ToListAsync();

            return await context.Set<TEntity>().AsNoTracking().ToListAsync();
        }

        public async Task<TEntity?> GetAsync(Tkey id)
             => await context.Set<TEntity>().FindAsync(id);

        #region Specification pattern   
        public async Task<IEnumerable<TEntity>> GetAllAsync(Specification<TEntity> specification)
            => await ApplySpecification(specification).ToListAsync();

        public async Task<TEntity?> GetAsync(Specification<TEntity> specification)
            => await ApplySpecification(specification).FirstOrDefaultAsync();

        public async Task<int> CountAsync(Specification<TEntity> specification)
            => await ApplySpecification(specification).CountAsync();

        private IQueryable<TEntity> ApplySpecification(Specification<TEntity> specification)
            => SpecificationEvaluator.GetQuery(context.Set<TEntity>(), specification);

        #endregion
    }
}
