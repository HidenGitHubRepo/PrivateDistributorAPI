using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeFirst.Model;
using Microsoft.Win32;

namespace PrivateDistributor.Data
{
    public class DistributorContext : DbContext
    {
        public DistributorContext()
            : base("DistributorDb")
        {
        }

        public DbSet<User> Users { get; set; }

        public DbSet<Car> Cars { get; set; }
    }
}
