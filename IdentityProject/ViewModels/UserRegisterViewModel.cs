using System.ComponentModel.DataAnnotations;

namespace IdentityProject.ViewModels
{
    public class UserRegisterViewModel
    {
        [Display(Name ="Adınız")]
        [Required(ErrorMessage ="Adınız Boş Geçilemez")]
        public string FirstName { get; set; }

        [Display(Name = "Soyadınız")]
        [Required(ErrorMessage = "Soyadınız Boş Geçilemez")]
        public string LastName { get; set; }

        [Display(Name = "Kullanıcı Adınız")]
        [Required(ErrorMessage = "Kullanıcı Adınız Boş Geçilemez")]
        public string UserName { get; set; }

        [Display(Name = "Email Adresiniz")]
        [Required(ErrorMessage = "Email Adresiniz Boş Geçilemez")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Display(Name = "Şifreniz")]
        [Required(ErrorMessage = "Şifreniz  Boş Geçilemez")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

    }
}
