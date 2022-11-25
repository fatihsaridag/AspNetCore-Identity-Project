using System.ComponentModel.DataAnnotations;

namespace IdentityProject.ViewModels
{
    public class PasswordChangeViewModel
    {
        [Display(Name ="Şifreniz")]
        [Required(ErrorMessage ="Eski şifreniz gereklidir")]
        [DataType(DataType.Password)]
        [MinLength(4,ErrorMessage ="Şifreniz en az 4 karakterli olmak zorundadır")]
        public string OldPassword { get; set; }
        [Display(Name = "Eski Şifreniz")]
        [Required(ErrorMessage ="Eski şifreniz gereklidir")]
        [DataType(DataType.Password)]
        [MinLength(4, ErrorMessage = "Şifreniz en az 4 karakterli olmak zorundadır")]
        public string NewPassword { get; set; }
        [Display(Name = "Şifreniz")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Eski şifreniz gereklidir")]
        [Compare("NewPassword",ErrorMessage ="Yeni şifreniz ve onay şifreniz birbirinden farklıdır.")]
        [MinLength(4, ErrorMessage = "Şifreniz en az 4 karakterli olmak zorundadır")]
        public string PasswordConfirm { get; set; }
    }
}
