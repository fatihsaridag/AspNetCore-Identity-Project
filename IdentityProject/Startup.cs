using IdentityProject.CutomValidation;
using IdentityProject.Models;
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

namespace IdentityProject
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
            services.AddRazorPages();
            services.AddMvc();

            services.AddDbContext<IdentityProjectContext>(opts =>
            {
                opts.UseSqlServer(Configuration["ConnectionStrings:DefaultConnectionString"]);
            });


            services.AddIdentity<AppUser, AppRole>(opts =>
            {
                opts.Password.RequiredLength = 4;
                opts.Password.RequireNonAlphanumeric = false;
                opts.Password.RequireLowercase = false;
                opts.Password.RequireUppercase = false;
                opts.Password.RequireDigit = false;


                opts.User.AllowedUserNameCharacters = "abcdefghýijklmnoöpqrsþtuüvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._";
                opts.User.RequireUniqueEmail = true;


            }).AddErrorDescriber<CustomIdentityErrorDescriber>().AddEntityFrameworkStores<IdentityProjectContext>().AddDefaultTokenProviders();


            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = new PathString("/Home/SignIn");
                options.LogoutPath = new PathString("/Member/Logout");
                options.AccessDeniedPath = new PathString("/Member/AccessDenied");
                options.Cookie = new CookieBuilder
                {
                    Name = "IdentityProject",
                    HttpOnly= true,
                    SameSite = SameSiteMode.Strict,
                    SecurePolicy = CookieSecurePolicy.SameAsRequest
                };
                options.SlidingExpiration = true;
                options.ExpireTimeSpan = System.TimeSpan.FromDays(7);
            });





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
                app.UseExceptionHandler("/Error");
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
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
