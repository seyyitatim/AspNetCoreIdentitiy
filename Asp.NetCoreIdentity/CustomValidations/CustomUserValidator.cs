using Asp.NetCoreIdentity.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Asp.NetCoreIdentity.CustomValidations
{
    public class CustomUserValidator : IUserValidator<AppUser>
    {
        public Task<IdentityResult> ValidateAsync(UserManager<AppUser> manager, AppUser user)
        {
            List<IdentityError> errors = new List<IdentityError>();

            string[] Digits = new string[] { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9",".","-","_" };

            foreach (var digit in Digits)
            {
                if (user.UserName[0].ToString()==digit)
                {
                    errors.Add(new IdentityError()
                    {
                        Code = "UserNameContainsFirstLetterDigitContains",
                        Description = "Kullanıcı adı sayı ya da noktalama işaretleriyle başlayamaz."
                    });
                }
            }

            if (errors.Count == 0)
            {
                return Task.FromResult(IdentityResult.Success);
            }
            else
            {
                return Task.FromResult(IdentityResult.Failed(errors.ToArray()));
            }
        }
    }
}
