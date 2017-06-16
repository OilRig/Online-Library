using System.ComponentModel.DataAnnotations;

namespace OnlineLibrary.DAL.Entities
{
    public class Book
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Author { get; set; }
        public string Genre { get; set; }
        public string Publisher { get; set; }
        public string Description { get; set; }
        public byte[] Image { get; set; }
    }
}
