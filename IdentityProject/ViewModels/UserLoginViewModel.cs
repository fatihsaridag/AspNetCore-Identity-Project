using System.ComponentModel.DataAnnotations;

namespace IdentityProject.ViewModels
{
    public class UserLoginViewModel
    {
        [Display(Name ="Email")]
        [Required(ErrorMessage ="Lüften email adresinizi giriniz")]
        public string Email { get; set; }

        [Display(Name = "Şifre")]
        [Required(ErrorMessage = "Lüften şifrenizi giriniz")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
