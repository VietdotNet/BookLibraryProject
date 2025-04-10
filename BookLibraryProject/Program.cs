﻿using BookLibraryProject.Repositories;
using BookLibraryProject.Services;
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

            // Lấy MailSettings từ cấu hình (appsettings.json)
            builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));

            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<UserService>();

            builder.Services.AddScoped<IBookCategoryRepository, BookCategoryRepository>();
            builder.Services.AddScoped<BookCategoryService>();

            builder.Services.AddScoped<IBookRepository, BookRepository>();
            builder.Services.AddScoped<BookService>();

            builder.Services.AddScoped<IBorrowRecordRepository, BorrowRecordRepository>();
            builder.Services.AddScoped<BorrowRecordService>();

            builder.Services.AddTransient<SendMailService>();
            builder.Services.AddHostedService<OverdueCheckService>();
            builder.Services.AddHostedService<OverdueBorrowService>();


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
