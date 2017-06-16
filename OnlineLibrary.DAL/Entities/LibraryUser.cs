using Microsoft.AspNet.Identity.EntityFramework;

namespace OnlineLibrary.DAL.Entities
{
    public class LibraryUser : IdentityUser
    {
        public virtual ClientProfile ClientProfile { get; set; }
    }
}
