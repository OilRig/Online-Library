using System.ComponentModel.DataAnnotations;

namespace OnlineLibrary.Models
{
    public class BookViewModel
    {
        public int Id { get; set; }
        [Required]
        [Display(Name="Название")]
        public string Name { get; set; }
        [Required]
        [Display(Name = "Автор")]
        public string Author { get; set; }
        [Display(Name = "Жанр")]
        public string Genre { get; set; }
        [Required]
        [Display(Name = "Издательство")]
        public string Publisher { get; set; }
        [Required]
        [Display(Name = "Описание")]
        public string Description { get; set; }
        public bool? Order { get; set; }
        public string ReservUserName { get; set; }
        [Display(Name = "Изображение")]
        public byte[] Image { get; set; }
    }
}