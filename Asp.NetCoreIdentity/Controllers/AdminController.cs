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
    [Authorize(Roles ="Admin")]
    public class AdminController : BaseController
    {
        public AdminController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<AppRole> roleManager) : base(userManager, signInManager, roleManager)
        {
        }
        public IActionResult Index()
        {
            return View(new UserListDto()
            {
                Users = userManager.Users.ToList()
            });
        }
        public IActionResult Users()
        {
            var users = userManager.Users.ToList();
            UserListDto model = new UserListDto()
            {
                Users = users
            };
            return View(model);
        }
        public IActionResult Roles()
        {
            var roles = roleManager.Roles;

            var model = roles.ToList();

            return View(model);
        }
        public IActionResult RoleDelete(string id)
        {
            var role = roleManager.FindByIdAsync(id).Result;

            if (role != null)
            {
                var result = roleManager.DeleteAsync(role).Result;
            }
            return RedirectToAction("Roles");
        }
        public IActionResult RoleCreate()
        {
            return View();
        }
        [HttpPost]
        public IActionResult RoleCreate(RoleCreateViewModel model)
        {
            AppRole role = new AppRole()
            {
                Name = model.Name
            };

            var result = roleManager.CreateAsync(role).Result;

            if (result.Succeeded)
            {
                return RedirectToAction("Roles");
            }
            AddModelError(result);

            return View(model);
        }
        public IActionResult RoleEdit(string id)
        {
            var role = roleManager.FindByIdAsync(id).Result;
            if (role == null)
            {
                return RedirectToAction("Roles");
            }
            var model = role.Adapt<RoleEditViewModel>();
            return View(model);
        }

        [HttpPost]
        public IActionResult RoleEdit(RoleEditViewModel model)
        {
            var role = roleManager.FindByIdAsync(model.Id).Result;
            if (role != null)
            {
                role.Name = model.Name;

                var result = roleManager.UpdateAsync(role).Result;

                if (result.Succeeded)
                {
                    ViewBag.success = true;
                    return View(role.Adapt<RoleEditViewModel>());
                }
                else
                {
                    AddModelError(result);
                    return View(role.Adapt<RoleEditViewModel>());
                }
            }
            ModelState.AddModelError("", "Güncelleme işlemi başarısız oldu.");
            return RedirectToAction("Roles");
        }

        public IActionResult RoleAssign(string id)
        {
            TempData["userId"] = id;
            var user = userManager.FindByIdAsync(id).Result;

            ViewBag.username = user.UserName;

            var roles = roleManager.Roles.ToList();

            var userRoles = userManager.GetRolesAsync(user).Result;

            List<RoleAssignViewModel> roleAssignViewModels = new List<RoleAssignViewModel>();

            foreach (var role in roles)
            {
                var roleAssignViewModel = new RoleAssignViewModel()
                {
                    RoleId = role.Id,
                    RoleName = role.Name
                };
                if (userRoles.Contains(role.Name))
                {
                    roleAssignViewModel.Exist = true;
                }
                else
                {
                    roleAssignViewModel.Exist = false;
                }
                roleAssignViewModels.Add(roleAssignViewModel);
            }

            return View(roleAssignViewModels);
        }

        [HttpPost]
        public async Task<IActionResult> RoleAssign(List<RoleAssignViewModel> model)
        {
            var user = userManager.FindByIdAsync(TempData["userId"].ToString()).Result;
            foreach (var role in model)
            {
                if (role.Exist)
                {
                    await userManager.AddToRoleAsync(user, role.RoleName);
                }
                else
                {
                    await userManager.RemoveFromRoleAsync(user, role.RoleName);
                }
            }
            return RedirectToAction("Users");
        }
    }
}
