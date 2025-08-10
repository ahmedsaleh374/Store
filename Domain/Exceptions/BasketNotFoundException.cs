using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
    public class BasketNotFoundException : Exception
    {
        public BasketNotFoundException(string id ):base($"Basket with {id} is not found ")
        {
            
        }
    }
}
