using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeFirst.Model;

namespace PrivateDistributor.Data
{
    public class DistributorContext : DbContext
    {
        public DistributorContext()
            : base("DistributorDb")
        {
        }

        public DbSet<   User            >   Users               { get; set; }
        public DbSet<   Car             >   Cars                { get; set; }
        public DbSet<   Company         >   Companies           { get; set; }
        public DbSet<   Delivery        >   Deliveries          { get; set; }
        public DbSet<   NewUserAuthCode >   NewUserAuthCodes    { get; set; }
        public DbSet<   Product         >   Products            { get; set; }
        public DbSet<   Order           >   Orders              { get; set; }
        public DbSet<   Phone           >   Phones              { get; set; }
        public DbSet<   Email            >   Mails               { get; set; }

        /*

        Company
        Delivery
        NewUserAuthCode
        Product
        Order
        */




    }
}
