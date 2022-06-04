using Etaa.Data;
using Etaa.Data.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Serilog;
using Serilog.Sinks.MSSqlServer;

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
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            // Services Configuration
            services.AddScoped<IFamiliesService, FamiliesService>();

            services.AddControllersWithViews();

            services.AddAntiforgery(o => o.HeaderName = "XSRF-TOKEN");

            services.AddSingleton<IFileProvider>(
                new PhysicalFileProvider(
                    Path.Combine(Directory.GetCurrentDirectory(),
                    "./wwwroot/ProjectFiles")));

            // For the users
            services.AddHttpContextAccessor();
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

            app.UseAuthorization();

            app.UseSession();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                        name: "default",
                        pattern: "{controller=Families}/{action/Index}/{id?}"
                    );
            });

            //var logDB = @"Server=DESKTOP-SNISS5A\\SQLEXPRESS;Database=Etaa;Trusted_Connection=True;MultipleActiveResultSets=true";
            //var sinkOpts = new MSSqlServerSinkOptions();
            //sinkOpts.TableName = "Logs";
            //Log.Logger = new LoggerConfiguration()
            //    .ReadFrom.Configuration(Configuration)
            //    .Enrich.FromLogContext()
            //    .WriteTo.MSSqlServer(
            //        connectionString: logDB,
            //        sinkOptions: sinkOpts
            // )
            // .CreateLogger();

            //Log.Logger = new LoggerConfiguration()
            //            .WriteTo
            //            .MSSqlServer(
            //                connectionString: "Server=DESKTOP-SNISS5A\\SQLEXPRESS;Database=Etaa;Trusted_Connection=True;MultipleActiveResultSets=true",
            //                sinkOptions: new MSSqlServerSinkOptions { TableName = "Logs" })
            //            .CreateLogger();

            //loggerFactory.AddSerilog();

            //new AppProperties(app.Properties).OnAppDisposing.Register(Log.CloseAndFlush);

            //app.UseSerilogRequestLogging();

            //IHostBuilder CreateHostBuilder(string[] args) =>
            //Host.CreateDefaultBuilder(args)
            //.UseSerilog()
            //.ConfigureWebHostDefaults(webBuilder =>
            //{
            //    webBuilder.UseStartup<Etaa.Startup>();
            //});

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

            //app.UseSerilogRequestLogging();
        }
    }
}
