using OnlineLibrary.BLL.Interfaces;
using OnlineLibrary.DAL.Repositories;

namespace OnlineLibrary.BLL.Services
{
    public class ServiceCreator : IServiceCreator
    {
        public IUserService CreateUserService(string connection)
        {
            return new UserService(new UnitOfWork(connection));
        }
    }
}
