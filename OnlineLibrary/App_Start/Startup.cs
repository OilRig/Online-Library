using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using OnlineLibrary.BLL.Interfaces;
using OnlineLibrary.BLL.Services;
using Owin;
using System;
using System.Threading;
using OnlineLibrary.jobs;

[assembly: OwinStartup(typeof(OnlineLibrary.App_Start.Startup))]

namespace OnlineLibrary.App_Start
{
    public class Startup
    {
        IServiceCreator serviceCreator = new ServiceCreator();
        public void Configuration(IAppBuilder app)
        {
            app.CreatePerOwinContext<IUserService>(CreateUserService);
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login"),
            });
        }

        private IUserService CreateUserService()
        {
            return serviceCreator.CreateUserService("LibraryContext");
        }
    }
}