using Asp.NetCoreIdentity.Entities;
using Asp.NetCoreIdentity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Asp.NetCoreIdentity.Controllers
{
    public class AdminController : BaseController
    {
        public AdminController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager) : base(userManager, signInManager)
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
    }
}
