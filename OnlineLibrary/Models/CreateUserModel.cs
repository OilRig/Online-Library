using System.ComponentModel.DataAnnotations;


namespace OnlineLibrary.Models
{
    public class CreateUserModel
    {

        [Required]
        [Display(Name = "Имя")]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        [StringLength(20, ErrorMessage = "Пароль должен содержать от 5 до 20 символов", MinimumLength = 5)]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        [Display(Name = "Повторите пароль")]
        public string ConfirmPassword { get; set; }
        [Required]
        [Display(Name = "Адрес")]
        [StringLength(40, ErrorMessage = "Не более 40 символов")]
        public string Address { get; set; }
        [Required]
        [Display(Name = "Роль")]
        public string Role { get; set; }

    }
}