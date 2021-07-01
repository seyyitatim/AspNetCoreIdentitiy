using Asp.NetCoreIdentity.CustomValidations;
using Asp.NetCoreIdentity.Data;
using Asp.NetCoreIdentity.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Asp.NetCoreIdentity
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(opt =>
            {
                opt.UseSqlServer(Configuration["ConnectionStrings:Default"]);
            });

            services.AddIdentity<AppUser, AppRole>(opt =>
            {
                opt.Password.RequiredLength = 4; // şifre uzunluğu
                opt.Password.RequireNonAlphanumeric = false; // (*,. vb.) karakterler zorunlu mu
                opt.Password.RequireLowercase = false; // küçük harf zorunlu mu
                opt.Password.RequireUppercase = false; // büyük harf zorunlu mu
                opt.Password.RequireDigit = false; // sayı zorunlu mu

                opt.User.RequireUniqueEmail = true; // email adresi tek olması
                opt.User.AllowedUserNameCharacters = "abcçdefghıijklmnoöpqrsştuüvwxyzABCÇDEFGHIİJKLMNOPQRSŞTUÜVWXYZ0123456789-._"; // izin verilen karakterler
            })
                .AddPasswordValidator<CustomPasswordValidator>()
                .AddUserValidator<CustomUserValidator>()
                .AddErrorDescriber<CustomIdentityErrorDescriber>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.ConfigureApplicationCookie(opt =>
            {
                opt.Cookie.Name = "MyBlog";
                opt.Cookie.HttpOnly = false;
                opt.Cookie.SameSite = SameSiteMode.Strict;
                opt.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
                opt.ExpireTimeSpan = TimeSpan.FromDays(2);
                opt.LoginPath = new PathString("/Home/Login");
                opt.SlidingExpiration = true;//geçerlilik süresini yarısına gelince, bir sonraki istekte süreyi uzatır.
            });

            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
