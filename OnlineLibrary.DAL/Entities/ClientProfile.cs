using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineLibrary.DAL.Entities
{
    public class ClientProfile
    {
        [Key]
        [ForeignKey("LibraryUser")]
        public string Id { get; set; }
        public string Name { get; set; }
        public string Adress { get; set; }
        public virtual LibraryUser  LibraryUser { get; set; }
    }
}
