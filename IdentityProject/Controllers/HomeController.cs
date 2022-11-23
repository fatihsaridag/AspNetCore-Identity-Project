using IdentityProject.Models;
using IdentityProject.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.Language;
using System;
using System.Threading.Tasks;

namespace IdentityProject.Controllers
{
    public class HomeController : Controller
    {

        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public HomeController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }


        public IActionResult Index()
        {
            return View();
        }


        [HttpGet]
        public IActionResult SignUp()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> SignUp(UserRegisterViewModel userRegisterViewModel)
        {
            if (ModelState.IsValid)
            {
                AppUser user = new AppUser();
                user.FirtName = userRegisterViewModel.FirstName;
                user.LastName = userRegisterViewModel.LastName;
                user.Email = userRegisterViewModel.Email;
                user.UserName = userRegisterViewModel.UserName;

                IdentityResult result = await _userManager.CreateAsync(user, userRegisterViewModel.Password);
                if (result.Succeeded)
                {
                    return RedirectToAction("SingIn");
                }
                else
                {
                    foreach(var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }

            return View(userRegisterViewModel);
        }


        [HttpGet]
        public IActionResult SignIn(string ReturnUrl)
        {
            TempData["ReturnUrl"] = ReturnUrl;
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> SignIn(UserLoginViewModel userLoginViewModel)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(userLoginViewModel.Email); //Önce bir veritabanında var mı yok mu ona bakıyoruz 
                if (user != null)
                {
                    if (await _userManager.IsLockedOutAsync(user))      //Eğer bu kullanıcı kilitli ise
                    {
                        ModelState.AddModelError("", "Hesabınız bir süreliğine kilitlenmiştir. Lütfen daha sonra tekrar deneyiniz. ");
                        return View(userLoginViewModel);


                    }

                    await _signInManager.SignOutAsync();    //Bizim önceden yazdıgımız cookie varsa onu bir önce silsin.
                    var result = await _signInManager.PasswordSignInAsync(user, userLoginViewModel.Password, userLoginViewModel.RememberMe, false);
                    if (result.Succeeded)
                    {
                        await _userManager.ResetAccessFailedCountAsync(user);   // Veritabanındaki AccessFailedCount değerini sıfırladık


                        if (TempData["ReturnUrl"]!= null)
                        {
                            return Redirect(TempData["ReturnUrl"].ToString());
                        }
                        return RedirectToAction("Index", "Member");
                    }
                    else
                    {
                        //Eğer kullanıcı başarılı giriş yapamadıysa 
                        await _userManager.AccessFailedAsync(user);
                        int fail = await _userManager.GetAccessFailedCountAsync(user); // bu userın kaç başarısız giriş yaptığını aldık. 
                        ModelState.AddModelError("", $"{fail} kez başarısız giriş");
                        if (fail == 3)  //Eğer fail 3 olduysa 
                        {
                            await _userManager.SetLockoutEndDateAsync(user, new System.DateTimeOffset(DateTime.Now.AddMinutes(20)));
                            ModelState.AddModelError("", "Hesabınız 3 başarısız girişten dolayı 20 dakika süreyle kilitlenmiştir. Lütfen daha sonra tekrar deneyiniz.");

                        }
                        else
                        {
                            ModelState.AddModelError("", "Geçersiz email veya şifre");
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Bu email adresine kayıtlı kullanıcı bulunamamıştır.");
                }
            }

            return View(userLoginViewModel);
        }


    }
}
