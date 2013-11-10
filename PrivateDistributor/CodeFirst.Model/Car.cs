using System;
using System.Collections.Generic;
using System.Linq;
using CodeFirst.Model;

namespace CodeFirst.Model
{
    public class Car
    {
        public Car()
        {
            this.Extras = new HashSet<Extra>();
        }

        public int Id { get; set; }

        public string Maker { get; set; }

        public string Model { get; set; }

        public int ProductionYear { get; set; }

        public decimal Price { get; set; }

        public string Engine { get; set; }

        public string Gear { get; set; }

        public int HP { get; set; }
       
        public string ImageUrl { get; set; }

        public int Mileage { get; set; }

        public string Doors { get; set; }

        public string FuelType { get; set; }

        public int EngineVolume { get; set; }

        public virtual ICollection<Extra> Extras { get; set; }

        public virtual User Owner { get; set; }
    }
}
