namespace OnlineLibrary.BLL.DTO
{
    public class BookDTO
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
