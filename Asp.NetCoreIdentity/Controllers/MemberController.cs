using Asp.NetCoreIdentity.Entities;
using Asp.NetCoreIdentity.Enums;
using Asp.NetCoreIdentity.Models;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Asp.NetCoreIdentity.Controllers
{
    [Authorize]
    public class MemberController : BaseController
    {
        public MemberController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager) : base(userManager, signInManager)
        {
        }

        public IActionResult Index()
        {
            var user = CurrentUser;

            UserViewModel userModel = user.Adapt<UserViewModel>();

            return View(userModel);
        }

        public IActionResult PasswordChange()
        {
            return View();
        }

        [HttpPost]
        public IActionResult PasswordChange(PasswordChangeViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = CurrentUser;

                if (user != null)
                {
                    bool exist = userManager.CheckPasswordAsync(user, model.PasswordOld).Result;

                    if (exist)
                    {
                        var result = userManager.ChangePasswordAsync(user, model.PasswordOld, model.PasswordNew).Result;

                        if (result.Succeeded)
                        {
                            userManager.UpdateSecurityStampAsync(user);

                            signInManager.SignOutAsync();
                            signInManager.PasswordSignInAsync(user, model.PasswordNew, true, false);

                            ViewBag.success = true;
                        }
                        else
                        {
                            AddModelError(result);
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "Eski şifreniz yanlış");
                    }

                }
            }
            return View(model);
        }

        public IActionResult Edit()
        {
            var user = CurrentUser;

            var userModel = user.Adapt<UserEditViewModel>();

            ViewBag.Gender = new SelectList(Enum.GetNames(typeof(Gender)));


            return View(userModel);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(UserEditViewModel model)
        {
            ViewBag.Gender = new SelectList(Enum.GetNames(typeof(Gender)));
            if (ModelState.IsValid)
            {
                var user = CurrentUser;

                if (model.Image != null && model.Image.Length > 0)
                {
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(model.Image.FileName);

                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/user", fileName);

                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await model.Image.CopyToAsync(stream);

                        user.Picture = "/images/user/" + fileName;
                    }
                }



                user.UserName = model.UserName;
                user.Email = model.Email;
                user.PhoneNumber = model.PhoneNumber;
                user.City = model.City;
                user.BirthDay = model.BirthDay;
                user.Gender = (int)model.Gender;

                var result = await userManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    await userManager.UpdateSecurityStampAsync(user);

                    await signInManager.SignOutAsync();
                    await signInManager.SignInAsync(user, true);

                    ViewBag.success = true;
                }
                else
                {
                    AddModelError(result);
                }
            }
            return View(model);
        }

        [Authorize(Roles ="Editor")]
        public IActionResult Editor()
        {
            return View();
        }

        [Authorize(Roles = "Manager")]
        public IActionResult Manager()
        {
            return View();
        }

        public IActionResult AccessDenied(string ReturnUrl)
        {
            if (ReturnUrl.ToLower().Contains("violencegage"))
            {
                ViewBag.message = "Erişmeye çalıştığınız sayfa şiddet videoları içerdiğinden dolayı 15 yaşında büyük olmanız gerekmektedir";
            }
            else if (ReturnUrl.ToLower().Contains("ankarapage"))
            {
                ViewBag.message = "Bu sayfaya sadece şehir alanı ankara olan kullanıcılar erişebilir";
            }
            else if (ReturnUrl.ToLower().Contains("exchange"))
            {
                ViewBag.message = "30 günlük ücretsiz deneme hakkınız sona ermiştir.";
            }
            else
            {
                ViewBag.message = "Bu sayfaya erişim izniniz yoktur. Erişim izni almak için site yöneticisiyle görüşünüz";
            }

            return View();
        }
    }
}
