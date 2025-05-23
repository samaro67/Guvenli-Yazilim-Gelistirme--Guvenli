using Microsoft.EntityFrameworkCore;
using YemekTarifleri.Data;
using YemekTarifleri.Models;
using Microsoft.AspNetCore.Identity;

namespace YemekTarifleri
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // Identity + ROL sistemi aktif
            builder.Services.AddDefaultIdentity<ApplicationUser>()
                .AddRoles<IdentityRole>() // ?? Rolleri ekledik
                .AddEntityFrameworkStores<ApplicationDbContext>();

            // Razor Pages + MVC
            builder.Services.AddControllersWithViews();
            builder.Services.AddRazorPages();

            var app = builder.Build();

            // ROL OLUÞTURMA: Admin, Uye, Misafir
            using (var scope = app.Services.CreateScope())
            {
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                string[] roller = { "Admin", "Uye", "Misafir" };

                foreach (var rol in roller)
                {
                    if (!await roleManager.RoleExistsAsync(rol))
                    {
                        await roleManager.CreateAsync(new IdentityRole(rol));
                    }
                }
            }

            // HTTP pipeline
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication(); // ?? Giriþ iþlemleri için þart
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.MapRazorPages();

            app.Run();
        }
    }
}
