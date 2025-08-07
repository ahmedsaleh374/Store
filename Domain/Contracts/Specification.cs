using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Contracts
{
    public abstract class Specification<T> where T : class
    {
        public Expression<Func<T, bool>> Criteria { get; }

        protected Specification(Expression<Func<T, bool>> criteria)
        {

            Criteria = criteria;

        }

        public List<Expression<Func<T, object>>> Includes { get; } = new();
        protected void AddInclude(Expression<Func<T, object>> expression)
            => Includes.Add(expression);

        #region Pagination  
        public int Skip { get; private set; }
        public int Take { get; private set; }
        public bool IsPaginated { get; private set; }

        protected void ApplyPagination(int pageIndex, int pageSize)
        {
            IsPaginated = true;
            Take = pageSize;
            Skip = (pageIndex - 1) * pageSize;
        } 
        #endregion


        #region sorting 

        public Expression<Func<T, object>> OrderBy { get; private set; }
        public Expression<Func<T, object>> OrderByDescending { get; private set; }


        protected void SetOrderBy(Expression<Func<T, object>> orderBy)
            => OrderBy = orderBy;

        protected void SetOrderByDescending(Expression<Func<T, object>> orderByDescending)
            => OrderByDescending = orderByDescending; 
        #endregion


    }
}
