using Demo.BusinessLogic.Mappings;
using Demo.BusinessLogic.Services.Classes;
using Demo.BusinessLogic.Services.Interfaces;
using Demo.DataAccess.Data.Contexts;
using Demo.DataAccess.Repositories.Classes;
using Demo.DataAccess.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Demo.Presentation
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            //builder.Services.AddScoped<ApplicationDbContext>();

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
            {
                //options.UseSqlServer("Connection string");
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnectionString"));
                options.UseLazyLoadingProxies();
            });

            builder.Services.AddScoped<IDepartmentRepositorie, DepartmentRepositorie>();
            // When Anyone Ask you To Create Object From Class Implement IDepartmentRepositorie Interface ==> Create Instance From DepartmentRepositorie
            builder.Services.AddScoped<IDepartmentService, DepartmentService>();
            builder.Services.AddScoped<IEmployeeRepositorie, EmployeeRepositorie>();
            //builder.Services.AddAutoMapper(cfg => { }, typeof(MappingProfile).Assembly);
            builder.Services.AddAutoMapper(Mapping => Mapping.AddProfile(new MappingProfile()));
            builder.Services.AddScoped<IEmployeeService, EmployeeService>();



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

            app.UseAuthorization();  // Roles
            app.UseAuthentication(); // Login

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
