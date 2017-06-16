using Ninject;
using OnlineLibrary.BLL.Interfaces;
using OnlineLibrary.BLL.Services;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace OnlineLibrary.Util
{
    public class NinjectDependencyResolver : IDependencyResolver
    {
        private IKernel kernel;
        public NinjectDependencyResolver(IKernel kernelParam)
        {
            kernel = kernelParam;
            AddBindings();
        }
        public object GetService(Type serviceType)
        {
            return kernel.TryGet(serviceType);
        }
        public IEnumerable<object> GetServices(Type serviceType)
        {
            return kernel.GetAll(serviceType);
        }
        private void AddBindings()
        {
            kernel.Bind<IReservService>().To<ReservService>();
            kernel.Bind<IBookService>().To<BookService>();
            kernel.Bind<IGenreService>().To<GenreService>();
            kernel.Bind<IEmailService>().To<EmailService>();
        }
    }
}