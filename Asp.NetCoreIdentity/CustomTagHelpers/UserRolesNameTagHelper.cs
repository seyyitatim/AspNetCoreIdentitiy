using Asp.NetCoreIdentity.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Asp.NetCoreIdentity.CustomTagHelpers
{
    [HtmlTargetElement("td", Attributes = "user-roles")]
    public class UserRolesNameTagHelper : TagHelper
    {
        public UserManager<AppUser> UserManager { get; set; }

        public UserRolesNameTagHelper(UserManager<AppUser> userManager)
        {
            UserManager = userManager;
        }

        [HtmlAttributeName("user-roles")]
        public string UserId { get; set; }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var user = await UserManager.FindByIdAsync(UserId);

            var roles = await UserManager.GetRolesAsync(user);

            string html = string.Empty;

            foreach (var role in roles)
            {
                html += $"<span class='badge badge-info'>{role}</span>";
            }

            output.Content.SetHtmlContent(html);
        }
    }
}
