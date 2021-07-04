using Asp.NetCoreIdentity.Entities;
using Asp.NetCoreIdentity.Models;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Asp.NetCoreIdentity.Controllers
{
    [Authorize]
    public class MemberController : Controller
    {
        private readonly UserManager<AppUser> userManager;
        private readonly SignInManager<AppUser> signInManager;

        public MemberController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        public IActionResult Index()
        {
            var user = userManager.FindByNameAsync(User.Identity.Name).Result;

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
                var user = userManager.FindByNameAsync(User.Identity.Name).Result;

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
                            foreach (var error in result.Errors)
                            {
                                ModelState.AddModelError("", error.Description);
                            }
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
            var user = userManager.FindByNameAsync(User.Identity.Name).Result;

            var userModel = user.Adapt<UserEditViewModel>();

            return View(userModel);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(UserEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = userManager.FindByNameAsync(User.Identity.Name).Result;

                user.UserName = model.UserName;
                user.Email = model.Email;
                user.PhoneNumber = model.PhoneNumber;

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
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }
            return View(model);
        }
    }
}
