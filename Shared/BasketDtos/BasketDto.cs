using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.BasketDtos
{
    public class BasketDto
    {
        public string Id { get; set; }
        public IEnumerable<ItemDto> Items { get; set; }
    }
}
