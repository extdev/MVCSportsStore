using System;
using System.Collections.Generic;
using System.Web.Mvc;
using MVCSportsStore.Domain.Abstract;
using MVCSportsStore.Domain.Concrete;
using Ninject;

namespace MVCSportsStore.Infrastructure
{
    public class NinjectDependencyResolver : IDependencyResolver
    {
        private readonly IKernel _kernel;

        public NinjectDependencyResolver(IKernel kernel)
        {
            _kernel = kernel;
            AddBindings();
        }

        public object GetService(Type serviceType)
        {
            return _kernel.TryGet(serviceType);
        }
        public IEnumerable<object> GetServices(Type serviceType)
        {
            return _kernel.GetAll(serviceType);
        }

        private void AddBindings()
        {
            //Mock<IProductRepository> products = new Mock<IProductRepository>();
            //products.Setup(x => x.Products).Returns(new List<Product>
            //{
            //    new Product {Name = "Football", Price = 25 },
            //    new Product {Name = "Surf board", Price = 179 },
            //    new Product {Name = "Running shoes", Price = 95 },
            //});
            
            _kernel.Bind<IProductRepository>().To<EFProductRepository>();
        }
    }
}