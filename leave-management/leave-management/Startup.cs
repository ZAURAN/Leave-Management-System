using AutoMapper;
using leave_management.Contracts;
using leave_management.Data;
using leave_management.Mappings;
using leave_management.Repository;
using leave_management.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace leave_management
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            
            services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(
                Configuration.GetConnectionString("DefaultConnection")));

            

            services.AddScoped<ILeaveTypeRepository, LeaveTypeRepository>();
            services.AddScoped<ILeaveRequestRepository, LeaveRequestRepository>();
            services.AddScoped<ILeaveAllocationRepository, LeaveAllocationRepository>();
            services.AddScoped<IEmployeeService, EmployeeService>();

            services.AddTransient<IUnitOfWork, UnitOfWork>();

            //Email Settings Section
            services.Configure<EmailSettings>(Configuration.GetSection("EmailSettings"));
            services.AddSingleton<IEmailSender, EmailSender>();

            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new Maps());
            });

            IMapper mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);

            services.AddDefaultIdentity<Employee>()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddControllersWithViews();
            services.AddRazorPages();
        }

        public void Configure(
            IApplicationBuilder app, 
            IWebHostEnvironment env,
            UserManager<Employee> userManager,
            RoleManager<IdentityRole> roleManager
        )
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                context.Database.Migrate();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            SeedData.Seed(userManager, roleManager);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                //endpoints.MapControllerRoute(
                //name: "Employee",
                //pattern: "{controller=Employee}/{action=Create}/{id?}");
                endpoints.MapRazorPages();
            });
        }
    }
}
