using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public abstract class BaseEntity<TKey>
    {
        public TKey Id { get; set; }

        #region Audit tables 

        //public DateTime? CreatedAt { get; set; } = DateTime.Now;

        //public TKey? CreatedBy { get; set; }

        //public DateTime? UpdatedAt { get; set; }

        //public TKey? UpdatedBy { get; set; }

        //public DateTime? DeletedAt { get; set; }

        //public TKey? DeletedBy{ get; set; }

        //public bool? IsDeleted { get; set; }

        #endregion
    }
}
