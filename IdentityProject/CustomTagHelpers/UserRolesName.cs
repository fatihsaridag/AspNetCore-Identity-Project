using IdentityProject.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityProject.CustomTagHelpers
{
    [HtmlTargetElement("td", Attributes = "user-roles")]             //Hem td si olan hemde attributesi user-roles olan tagı yakaladık.
    public class UserRolesName : TagHelper
    {
        public UserManager<AppUser> UserManager { get; set; }      //Dependency Injection ile Kullanıcıyı almak için UserManagerı  getiriyoruz.
        [HtmlAttributeName("user-roles")]                           //UserId propertysini viewdeki user-roles attributesi ile eşleştirmek için
        public string UserId { get; set; }                          //UserId yi yakalamak için



        public UserRolesName(UserManager<AppUser> userManager)
        {
            this.UserManager = userManager;
        }


        // Bu methodun yaptığı outputa vermiş oldugumuz html veya string ifadeyi viewdeki td içindeki boş alana basıyor.
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)     //Tag helperın içeriğini doldurmak için override ettik
        {
            var user = await UserManager.FindByIdAsync(UserId);               //Userımızı aldık (UserId mizide üst tarafta tanımlayıp bağladık).
            var roles = await UserManager.GetRolesAsync(user);                // Userımıza ait rolümüzü yakaladık. List olarak döndü

            string html = string.Empty;                                       //Htmlimizi yazıcaz. Şu an boş olsun
            roles.ToList().ForEach(x =>                                       //Her bir rolümüzü html içerisinde yazması için  aşağıdaki kodları yazdık.
            {

                html += $"<span class = 'badge text-info'>{x}</span>";

            });

            output.Content.SetHtmlContent(html);    //output ile htmlimizi gönderiyoruz.
        }


    }
}
