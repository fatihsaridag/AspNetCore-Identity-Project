using IdentityProject.Models;
using IdentityProject.ViewModels;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityProject.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;

        public AdminController(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }


        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Users()
        {
            var userList = _userManager.Users.ToList();
            return View(userList);
        }

        [HttpGet]
        public IActionResult RoleCreate()
        {
            return View();
        }

        [HttpPost]
        public IActionResult RoleCreate(RoleViewModel roleViewModel)
        {
            AppRole role = new AppRole();
            role.Name = roleViewModel.Name;
            IdentityResult result = _roleManager.CreateAsync(role).Result;
            if (result.Succeeded)
            {
                return RedirectToAction("Roles");
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            return View(roleViewModel);
        }


        public IActionResult Roles()
        {
            var roles = _roleManager.Roles.ToList();

            return View(roles);

        }

        public IActionResult RoleDelete(string id)
        {
           var role = _roleManager.FindByIdAsync(id).Result;
            if (role != null)
            {
                IdentityResult result = _roleManager.DeleteAsync(role).Result;
            }
            return RedirectToAction("Roles");
        }

        [HttpGet]
        public async Task<IActionResult> RoleUpdate(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role != null)
            {
                var userViewModel = role.Adapt<RoleViewModel>();
                return View(userViewModel);
            }
            return RedirectToAction("Roles");
        }


        [HttpPost]
        public async Task<IActionResult> RoleUpdate(RoleViewModel roleViewModel)
        {
            var role = await _roleManager.FindByIdAsync(roleViewModel.Id);
            if (role != null)
            {
                role.Name = roleViewModel.Name;
                var result = await _roleManager.UpdateAsync(role);
                if (result.Succeeded)
                {
                    return RedirectToAction("Roles");
                }
                else
                {
                    foreach (var item in result.Errors)
                    {
                        ModelState.AddModelError("", item.Description);
                    }
                }
            }
            else
            {
                ModelState.AddModelError("", "Güncelleme işlemi başarısız oldu");
            }
            return View(roleViewModel);

        }

        [HttpGet]
        public async Task<IActionResult> RoleAssign(string id)
        {
            TempData["userId"] = id;
            var user = await _userManager.FindByIdAsync(id);                        //Id ye göre kullanıcıyı getiriyoruz.
            ViewBag.userName = user.UserName;                                       // Bu username yi view tarafına yazıcaz

            var roles = _roleManager.Roles;                                         //Rolleri çektik
            var userRoles = await _userManager.GetRolesAsync(user) as List<string>;  //Tıklamış olduğumuz kullanıcı hangi rollere saghip bunu List<string> olarak bize dönecek.
            var roleAssignViewModels = new List<RoleAssignViewModel>();              //RoleAssignViewModelden bir liste alıyoruz.

            foreach (var role in roles)                                               //
            {
                RoleAssignViewModel r = new RoleAssignViewModel();                    //Checkboxun işaretli olup olmadıgını bilmek içinn bir viewmodel yazdık ve doldurduk içini
                r.RoleId = role.Id;                                                  //viewmodel id içerisine at 
                r.RoleName = role.Name;                                              //viewmodel name içerisine at 
                if (userRoles.Contains(role.Name))                                   // Eğer kullanıcı rollerin içerisinde  bu kullanıcı var mı eğer var ise
                {
                    r.Exist = true;                                                  //Checkboxu işaretle
                }
                else
                {
                    r.Exist = false;                                                  //Yok ise chechboxu işaretleme
                }
                roleAssignViewModels.Add(r);
            }

            return View(roleAssignViewModels);
        }

        [HttpPost]
        public async Task<IActionResult> RoleAssign(List<RoleAssignViewModel> roleAssignViewModels)
        {
            var user = await _userManager.FindByIdAsync(TempData["userId"].ToString());     // TempDatada Get metodunda atayacağımız kullanıcıyı bulmak için id yi yakaladık ve kullanıcıyı bulduk
            foreach (var item in roleAssignViewModels)                                       //Listeyi foreach ile döndük                 
            {
                if (item.Exist)                                                             //Eğer checkboxa tıklanmış ise biz bu rolü atayalım
                {
                    await _userManager.AddToRoleAsync(user, item.RoleName);                  //1. Parametre user , 2. parametre rol adı
                }
                else
                {
                    await _userManager.RemoveFromRoleAsync(user, item.RoleName);            //Eğer checkbox işaretli değil ise bu rolü kaldırdık.
                }
            }

            return RedirectToAction("Users");                                               //Bu işlemler bitince "Users" sayfasına dön
        }


        public IActionResult Claims()
        {
            return View(User.Claims.ToList());
        }


    }
}
