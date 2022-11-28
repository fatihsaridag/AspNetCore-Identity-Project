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

            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Member");
            }
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
                user.FirstName = userRegisterViewModel.FirstName;
                user.LastName = userRegisterViewModel.LastName;
                user.Email = userRegisterViewModel.Email;
                user.UserName = userRegisterViewModel.UserName;

                IdentityResult result = await _userManager.CreateAsync(user, userRegisterViewModel.Password);
                if (result.Succeeded)
                {
                    string confirmationToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    string link = Url.Action("ConfirmEmail", "Home", new
                    {
                        userId = user.Id,
                        token = confirmationToken
                    }, protocol:HttpContext.Request.Scheme
                    );
                    Helper.EmailConfirmation.SendEmail(link, user.Email);   //Kime göndericez ?


                    return RedirectToAction("SignIn","Home");
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
                    if (!await _userManager.IsEmailConfirmedAsync(user))
                    {
                        ModelState.AddModelError("", "Email Adresiniz onaylanmamıştır. Lütfen e-postanızı kontrol ediniz");
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




        [HttpGet]
        public IActionResult ResetPassword()
        {
            return View();
        }



        [HttpPost]
        public async Task<IActionResult> ResetPassword(PasswordResetViewModel passwordResetViewModel)
        {
            // Böyle bir kullanıcı var mı? Test edelim
            AppUser user = await _userManager.FindByEmailAsync(passwordResetViewModel.Email);
            // böyle bir kullanıcı varsa 
            if (user != null)
            {
                // Token Oluşturuyoruz
                string passwordResetToken = _userManager.GeneratePasswordResetTokenAsync(user).Result;
                // Linki Burada gönderiyoruz Kullanıcı linke tıklayınca bu urle gelicek.
                string passworResetLink = Url.Action("ResetPasswordConfirm", "Home", new
                {
                    userId = user.Id,
                    token = passwordResetToken
                }, HttpContext.Request.Scheme);

                // deneme.com/Home/ResetPasswordConfirm?userId=jdkjasbhdtoken=kdlksamdl

                Helper.PasswordReset.PasswordResetSendEmail(passworResetLink, user.Email);
                ViewBag.status = "success";

            }
            // hata varsa
            else
            {
                ModelState.AddModelError("", "Sistemde kayıtlı email adresi bulunamamıştır!!!");
            }

            return View(passwordResetViewModel);
        }


        [HttpGet]
        public IActionResult ResetPasswordConfirm(string userId , string token) //deneme.com/Home/ResetPasswordConfirm? userId = jdkjasbhdtoken = kdlksamdl
        {
            //yukarıdaki url deki parametreleri tempdata ile yakaladık ve posta actionununa göndereceğiz. 
            TempData["userId"] = userId;
            TempData["token"] = token;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ResetPasswordConfirm([Bind("PasswordNew")]PasswordResetViewModel passwordResetViewModel)
        {
            string token = TempData["token"].ToString();        //Tokenı aldık
            string userId = TempData["userId"].ToString();      //UserId yi aldık 

            AppUser user = await _userManager.FindByIdAsync(userId);        //userId ile böyle bir kullanıcı var mı yok mu kontrol ediyoruz. 
            if (user != null)                                               // Eğer böyle bir kullanıcı var ise 
            {
                IdentityResult result = await _userManager.ResetPasswordAsync(user, token, passwordResetViewModel.PasswordNew); //IdentityResult ile şifre sıfırladık.
                if (result.Succeeded) //Eğer işlem başarılı ise 
                {
                    await _userManager.UpdateSecurityStampAsync(user);      //SecurityStampını güncelledik(user,password gibi alanlar değişirse)
                    TempData["passwordResetInfo"] = "Şifreniz başarıyla yenilenmiştir. Yeni şifreniz ile giriş yapabilirsiniz.";   
                    ViewBag.status = "success";  //Viewe gönderelim.İf kontrolü yapalım.
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
                ModelState.AddModelError("", "Hata meydana gelmiştir lütfen daha sonra tekrar deneyin.");
            }

            return View(passwordResetViewModel);
        }


        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            var user = await _userManager.FindByIdAsync(userId);
            IdentityResult result = await _userManager.ConfirmEmailAsync(user, token);
            if (result.Succeeded)
            {
                ViewBag.status = "Email adresiniz onaylanmıştır. Login ekranından giriş yapabilirsiniz.";
            }
            else
            {
                ViewBag.status = "Bir hata meydana geldi lütfen daha sonra tekrar deneyiniz. ";
            }

            return View();
        }


    }
}
