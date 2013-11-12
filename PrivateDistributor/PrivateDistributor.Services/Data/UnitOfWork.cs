using System;
using CodeFirst.Model;
using PrivateDistributor.Data;
using PrivateDistributor.Repositories;

namespace PrivateDistributor.Services.Data
{
    public class UnitOfWork : IDisposable
    {
        private DistributorContext context = new DistributorContext();
        public IRepository<User> userRepository { get; private set; }
        public IRepository<Car> carRepository { get; private set; }
        public IRepository<Extra> extraRepository { get; private set; }
        public IRepository<Company> companyRepository { get; private set; }
        public IRepository<Delivery> deliveryRepository { get; private set; }
        public IRepository<NewUserAuthCode> newUserAuthCodeRepository { get; private set; }
        public IRepository<Product> productRepository { get; private set; }
        public IRepository<Order> orderRepository { get; private set; }
        private bool disposed = false;

        public UnitOfWork()
        {
            this.userRepository = new EfRepository<User>(this.context);
            this.carRepository = new EfRepository<Car>(this.context);
            this.extraRepository = new EfRepository<Extra>(this.context);
            this.companyRepository          = new EfRepository<Company                    >(this.context);
            this.deliveryRepository         = new EfRepository<Delivery                   >(this.context);
            this.newUserAuthCodeRepository  = new EfRepository<NewUserAuthCode             >(this.context);
            this.productRepository          = new EfRepository<Product                     >(this.context);
            this.orderRepository            = new EfRepository<Order                       >(this.context);
        }

        public void Save()
        {
            this.context.SaveChanges();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    this.context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}