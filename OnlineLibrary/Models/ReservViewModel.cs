using System;
using System.ComponentModel.DataAnnotations;

namespace OnlineLibrary.Models
{
    public class ReservViewModel
    {
        public int Id { get; set; }
        [Display(Name = "Название книги")]
        public string BookName { get; set; }
        [Display(Name = "Email пользователя")]
        public string UserName { get; set; }
        [Display(Name ="Дана бронирования")]
        public DateTime Date { get; set; }
        [Display(Name = "Дана окончания брони")]
        public DateTime FinishDate { get; set; }
        [Display(Name ="Выдача")]
        public bool Resolution { get; set; }
    }
}