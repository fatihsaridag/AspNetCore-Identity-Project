using System.ComponentModel.DataAnnotations;

namespace IdentityProject.ViewModels
{
    public class PasswordResetViewModel
    {
        [Required]
        [Display(Name ="Email")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }


        [Display(Name = "Yeni Şifre")]
        [Required(ErrorMessage = "Lüften şifrenizi giriniz")]
        [DataType(DataType.Password)]
        [MinLength(4,ErrorMessage ="Şifreniz en az 4 karakterli olmalıdır.")]
        public string PasswordNew { get; set; }

    }
}
