using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.OrderEntities
{
    public class Order : BaseEntity<Guid>
    {
        public string BuyerEmail { get; set; }

        public Address ShippingAddress { get; set; }
        public OrderPaymentStatus PaymentStatus { get; set; } = OrderPaymentStatus.Pending;


        [ForeignKey("DeliveryMethod")]
        public int DeliveryMethodId { get; set; }
        public virtual DeliveryMethod? DeliveryMethod { get; set; }


        public virtual ICollection<OrderItem>? OrderItems { get; set; } = new List<OrderItem>();


        public DateTimeOffset Date { get; set; } = DateTimeOffset.Now;

        public  decimal SubTotal { get; set; }
    }
}
