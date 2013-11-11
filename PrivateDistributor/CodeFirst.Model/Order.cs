using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFirst.Model
{
    public class Order
    {
        public int Id { get; set; }

        public virtual Products Product { get; set; }
        public int Count { get; set; }


    }
}
