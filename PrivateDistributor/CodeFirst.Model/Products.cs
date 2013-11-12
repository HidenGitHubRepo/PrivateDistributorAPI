using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFirst.Model
{
    public class Product
    {
        public int Id { get; set; }

        public int PublicId { get; set; }

        public string  Name { get; set; }
        public string  Details { get; set; }

        public decimal Price { get; set; }

        public string ImageUrl { get; set; }

        public string Brand { get; set; }
        public string MadeIn { get; set; }
        public string  Category { get; set; }

        public int CountInWarehouse { get; set; }
    }
}
