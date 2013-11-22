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

        public string  PublicId { get; set; }

        public string Name { get; set; }
                
        public string NutritiveValue { get; set; }

        public string Description { get; set; }

        public string Storing { get; set; }

        public decimal Price { get; set; }

        public string ImageUrl { get; set; }

        public string ImageType { get; set; }

        public string Brand { get; set; }
        public string MadeIn { get; set; }
        public string Category { get; set; }

        public int CountInWarehouse { get; set; }
    }
}
