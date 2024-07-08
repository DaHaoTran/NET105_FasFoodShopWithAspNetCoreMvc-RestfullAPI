using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using UI.Areas.Identity.Data;
using System.Configuration;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;

namespace UI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var connectionString = builder.Configuration.GetConnectionString("FoodShopContextConnection") ?? throw new InvalidOperationException("Connection string 'FoodShopContextConnection' not found.");

            builder.Services.AddDbContext<Areas.Identity.Data.FoodShopContext>(options => options.UseSqlServer(connectionString));

            builder.Services.AddDefaultIdentity<FoodShopUser>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<FoodShopContext>();

            builder.Services.AddAuthentication().AddGoogle(options =>
            {
                IConfigurationSection googleAuthencation = builder.Configuration.GetSection("Authencation:Google");
                options.ClientId = googleAuthencation["ClientId"]!;
                options.ClientSecret = googleAuthencation["ClientSecret"]!;
            });

            // Add services to the container.
            builder.Services.AddSession();
            builder.Services.AddControllersWithViews();
            builder.Services.AddHttpClient();

            //builder.Services.AddSession(options =>
            //{
            //    options.Cookie.HttpOnly = true;
            //    options.Cookie.IsEssential = true;
            //});

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
            app.UseSession();

            app.UseAuthorization();
            app.UseAuthentication();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            app.MapRazorPages();

            app.Run();
        }
    }
}
