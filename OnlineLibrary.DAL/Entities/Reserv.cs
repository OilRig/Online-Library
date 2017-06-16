using System;

namespace OnlineLibrary.DAL.Entities
{
    public class Reserv
    {
        public int Id { get; set; }
        public string BookName { get; set; }
        public string UserName { get; set; }
        public DateTime Date { get; set; }
        public DateTime FinishDate { get; set; }
        public bool Resolution { get; set; }
    }
}
