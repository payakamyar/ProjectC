using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using ProjectC.Contracts;
using ProjectC.Data.Context;
using ProjectC.Data.Repository;
using ProjectC.Services;
using System.Security.Claims;

namespace ProjectC
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

			builder.Services.AddHttpContextAccessor();
			builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/Account/Login";
                    options.AccessDeniedPath = "/Forbidden";
                });

            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("Admin-Role", config =>
                {
                    config.RequireClaim(ClaimTypes.Role, "Admin");
                });
            });


            builder.Services.AddDbContext<ProjectContext>();

            builder.Services.AddScoped<IAuthentication,CookieAuthentication>();
			builder.Services.AddScoped<IUsersRepository, DapperUsersRepository>();
			builder.Services.AddScoped<RoleRepository>();
			builder.Services.AddScoped<AccountDataManager>();

            var app = builder.Build();


            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
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

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
