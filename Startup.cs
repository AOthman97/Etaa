using Etaa.Data;
using Etaa.Data.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;

namespace Etaa
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
            // Set the connection string of the app
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(Configuration
                .GetConnectionString("DefaultConnection")), ServiceLifetime.Transient);

            // Services Configuration
            //services.AddScoped<IFamiliesService, FamiliesService>();

            services.AddControllersWithViews();

            services.AddAntiforgery(o => o.HeaderName = "XSRF-TOKEN");

            services.AddSingleton<IFileProvider>(
                new PhysicalFileProvider(
                    Path.Combine(Directory.GetCurrentDirectory(),
                    "./wwwroot/ProjectFiles")));

            // For the IdentityUser
            services.AddHttpContextAccessor();

            services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false)
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Shared/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseDirectoryBrowser(new DirectoryBrowserOptions
                {
                    FileProvider = new PhysicalFileProvider
                (
                    Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "ProjectFiles")
                ),
                RequestPath = "/ProjectFiles"
            });

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseSession();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                        name: "default",
                        pattern: "{controller=Families}/{action/Index}/{id?}"
                    );
            });
        }
    }
}
