using Etaa.Data;
using Etaa.Data.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Serilog;
using Serilog.Events;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();

builder.Services.AddSingleton<IFileProvider>(
                new PhysicalFileProvider(
                    Path.Combine(Directory.GetCurrentDirectory(),
                    "./wwwroot/ProjectFiles")));

builder.Services.AddScoped<IFamiliesService, FamiliesService>();
builder.Services.AddScoped<IFamilyMembersService, FamilyMembersService>();
builder.Services.AddScoped<IContributorsService, ContributorsService>();

builder.Services.AddAntiforgery(o => o.HeaderName = "XSRF-TOKEN");

// For the IdentityUser
builder.Services.AddHttpContextAccessor();

builder.Services.AddSession();

//IHostBuilder CreateHostBuilder(string[] args) =>
//            Host.CreateDefaultBuilder(args)
//            .UseSerilog()
//            .ConfigureWebHostDefaults(webBuilder =>
//            {
//                webBuilder.UseStartup<Etaa.Startup>();
//            });

//var config = new ConfigurationBuilder()
//                                        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true).Build();

//Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(config).CreateLogger();

//try
//{
//    Log.Information("Application Started");
//    CreateHostBuilder(args).Build().Run();
//}
//catch (Exception ex)
//{
//    Log.Error(ex, "Application Failed to Start!");
//}
//finally
//{
//    Log.CloseAndFlush();
//}

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
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


app.UseRouting();
app.UseSession();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Families}/{action=Index}/{id?}");
app.MapRazorPages();

//app.UseSerilogRequestLogging();

app.Run();

//namespace Etaa
//{
//    public class Program
//    {
//        public static void Main(string[] args)
//        {
//            var config = new ConfigurationBuilder()
//                                        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true).Build();

//            Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(config).CreateLogger();

//            try
//            {
//                Log.Information("Application Started");
//                CreateHostBuilder(args).Build().Run();
//            }
//            catch (Exception ex)
//            {
//                Log.Error(ex, "Application Failed to Start!");
//            }
//            finally
//            {
//                Log.CloseAndFlush();
//            }
//        }

//        public static IHostBuilder CreateHostBuilder(string[] args) =>
//            Host.CreateDefaultBuilder(args)
//            .UseSerilog()
//            .ConfigureWebHostDefaults(webBuilder =>
//            {
//                webBuilder.UseStartup<Startup>();
//            });
//    }
//}
