using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.EntityFrameworkCore;

namespace BookLibraryProject
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddDbContext<BookLibraryManagementProjectContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("BookLibraryManagement_Project")));

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
            })
                .AddCookie(options =>
               {
                   options.LoginPath = "/Account/Login";  // Trang đăng nhập
                   options.AccessDeniedPath = "/Account/Logout"; // Trang bị từ chối
                   options.Cookie.HttpOnly = true;
                   options.ExpireTimeSpan = TimeSpan.FromMinutes(100);
                   // Gia hạn cookie nếu còn hoạt động
                   options.SlidingExpiration = true;
               })
               .AddGoogle(options =>
               {
                   var result = builder.Configuration.GetSection("Authentication:Google");
                   options.ClientId = result["ClientId"];
                   options.ClientSecret = result["ClientSecret"];
                   options.CallbackPath = "/signInGoogle";
               });

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
