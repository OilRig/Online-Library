using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using OnlineLibrary.DAL.Entities;

namespace OnlineLibrary.DAL.Identity
{
    public class LibraryRoleManager : RoleManager<LibraryRole>
    {
        public LibraryRoleManager(RoleStore<LibraryRole> store) : base(store)
        {}
    }
}
