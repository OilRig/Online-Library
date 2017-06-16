using Microsoft.AspNet.Identity;
using OnlineLibrary.DAL.Entities;

namespace OnlineLibrary.DAL.Identity
{
    public class LibraryUserManager : UserManager<LibraryUser>
    {
        public LibraryUserManager(IUserStore<LibraryUser> store) : base(store)
        {}
    }
}
