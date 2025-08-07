using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    public record PaginatedResult<TDate>(int pageIndex,int pageSize , int totalCount,IEnumerable<TDate> data);
    
}
