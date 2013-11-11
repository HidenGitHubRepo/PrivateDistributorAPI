using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFirst.Model
{
    public class Delivery
    {
        public int Id { get; set; }

        public string LocationDetails { get; set; }
        public DateTime DeliveryAtDate { get; set; }

        public virtual Company Company { get; set; }
        public virtual User Orderer { get; set; }
        public virtual User Assister { get; set; }

        public ICollection<Order> Orders { get; set; }

        public Delivery()
        {
            this.Orders = new HashSet<Order>();
            this.TotalPrice = 0m;
            this.IsDelivered = false;
        }

        public decimal TotalPrice { get; set; }
        public bool IsDelivered { get; set; }
    }
}
