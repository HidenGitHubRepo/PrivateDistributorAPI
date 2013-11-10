using System;
using System.Collections.Generic;
using System.Linq;

namespace CodeFirst.Model
{
    public class Extra
    {
        public Extra()
        {
            this.Cars = new HashSet<Car>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public virtual ICollection<Car> Cars { get; set; } 
    }
}
