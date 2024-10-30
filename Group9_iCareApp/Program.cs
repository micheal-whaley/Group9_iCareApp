using Group9_iCareApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace Group9_iCareApp
{
    public class Program()
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var connectionString = builder.Configuration.GetConnectionString("iCAREDBContextConnection") ?? throw new InvalidOperationException("Connection string 'iCAREDBContextConnection' not found.");

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddRazorPages();

            //CHANGE NAME OF SERVER TO YOUR SERVER NAME
            builder.Services.AddDbContext<iCAREDBContext>(option => option.UseSqlServer("Server=localhost\\MSSQLSERVER01;Database=Group9_iCareDB;Trusted_Connection=True;"));


            builder.Services.AddDefaultIdentity<iCAREUser>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<iCAREDBContext>();

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
            app.MapRazorPages();
            app.Run();
        }
    }
}
