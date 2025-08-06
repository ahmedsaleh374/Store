using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Contracts
{
    public interface IGenericRepository<TEntity,TKey> where TEntity : BaseEntity<TKey>
    {
        Task <IEnumerable<TEntity>> GetAllAsync(bool isTrackable =false);
        Task <IEnumerable<TEntity>> GetAllAsync(Specification<TEntity> specification);

        Task <TEntity?> GetAsync(TKey id);
        Task <TEntity?> GetAsync(Specification<TEntity> specification);

        Task AddAsync(TEntity entity);

        void Delete(TEntity entity);
        void Update(TEntity entity);


    }
}
