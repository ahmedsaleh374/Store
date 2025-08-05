using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Product : BaseEntity<int>
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public string PictureUrl { get; set; }

        public decimal Price { get; set; }

        [ForeignKey("ProductBrand")]
        public int BrandId { get; set; }
        public virtual ProductBrand? ProductBrand { get; set; }

        [ForeignKey("ProductType")]
        public int TypeId { get; set; }
        public virtual ProductType? ProductType { get; set; }

    }
}
