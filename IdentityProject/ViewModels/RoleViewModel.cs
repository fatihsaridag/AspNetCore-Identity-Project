using System.ComponentModel.DataAnnotations;

namespace IdentityProject.ViewModels
{
    public class RoleViewModel
    {
        public string Id { get; set; }
        [Display(Name="Role İsmi")]
        [Required(ErrorMessage ="Role ismi gereklidir.")]
        public string Name { get; set; }
    }
}
