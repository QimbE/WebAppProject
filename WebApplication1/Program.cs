using Google.Apis.Auth.AspNetCore3;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Microsoft.EntityFrameworkCore;
using Stripe;
using TestShop.DataAccess.Data;
using TestShop.DataAccess.Repository;
using TestShop.DataAccess.Repository.IRepository;
using TestShop.Models;
using TestShop.Utility;

namespace TestShopProject
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddRazorPages();

            //EF DbContext connect to server
            builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(
                builder.Configuration.GetConnectionString("DefaultConnection")
                ));

            //Stripe keys
            builder.Services.Configure<StripeSettings>(builder.Configuration.GetSection("Stripe"));

            //Identity
            builder.Services.AddIdentity<IdentityUser, IdentityRole>(/*options => options.SignIn.RequireConfirmedAccount = true*/)
	            .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            //Cookie routing
            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Identity/Account/Login";
                options.LogoutPath = "/Identity/Account/Logout";
                options.AccessDeniedPath = "/Identity/Account/AccessDenied";
            });

            //Google auth
            builder.Services
                .AddAuthentication(/*o =>
                    {
                        o.DefaultChallengeScheme = GoogleOpenIdConnectDefaults.AuthenticationScheme;
                        o.DefaultForbidScheme = GoogleOpenIdConnectDefaults.AuthenticationScheme;
                        o.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    }*/)
                .AddCookie()
                .AddGoogleOpenIdConnect(options =>
                {
                    options.ClientId = builder.Configuration.GetSection("Google").GetSection("ClientId");
                    options.ClientSecret = builder.Configuration.GetSection("Google").GetSection("ClientSecret");
                });


            //Session
            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(100);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            //Stub code
            builder.Services.AddScoped<IEmailSender, EmailSender>();

			//Repository DI
			builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            //Runtime code changes compilation
            builder.Services.AddRazorPages().AddRazorRuntimeCompilation();

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

            //Stripe connection.
            StripeConfiguration.ApiKey = builder.Configuration.GetSection("Stripe:SecretKey").Get<string>();

			app.UseRouting();

            app.UseAuthentication();

			app.UseAuthorization();

            app.UseSession();

            app.MapRazorPages();

            app.MapControllerRoute(
                name: "default",
                pattern: "{area=Customer}/{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}