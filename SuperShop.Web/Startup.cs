using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SuperShop.Web.Data;
using SuperShop.Web.Data.DataContext;
using SuperShop.Web.Data.Entities;
using SuperShop.Web.Data.Repositories;
using SuperShop.Web.Helpers;
using SuperShop.Web.Services;
using SuperShop.Web.Utils.ConfigOptions;

namespace SuperShop.Web;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    private IConfiguration Configuration { get; }


    //
    // This method gets called by the runtime.
    // Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddIdentity<User, IdentityRole>(
            cfg =>
            {
                cfg.User.RequireUniqueEmail = true;

                cfg.Password.RequireDigit = false;
                cfg.Password.RequiredLength = 6;
                cfg.Password.RequiredUniqueChars = 0;
                cfg.Password.RequireUppercase = false;
                cfg.Password.RequireLowercase = false;
                cfg.Password.RequireNonAlphanumeric = false;

                cfg.SignIn.RequireConfirmedEmail = false;
                cfg.SignIn.RequireConfirmedAccount = false;
                cfg.SignIn.RequireConfirmedPhoneNumber = false;
            }).AddEntityFrameworkStores<DataContextMSSQL>();


        // este é o por defeito, mas já está definido em cima
        //services.AddDefaultIdentity<IdentityUser>(
        //        options =>
        //            options.SignIn.RequireConfirmedAccount = true)
        //    .AddEntityFrameworkStores<DataContextMSSQL>();


        services.AddDbContext<DataContextMSSQL>(
            cfg =>
            {
                cfg.UseSqlServer(
                    Configuration.GetConnectionString("SomeeConnection"),
                    options =>
                    {
                        options.EnableRetryOnFailure();
                        options.MigrationsAssembly("SuperShop.Web");
                        options.MigrationsHistoryTable("_MyMigrationsHistory");
                    });
            });


        services.AddDbContext<DataContextMySQL>(
            cfg =>
            {
                cfg.UseMySQL(
                    Configuration.GetConnectionString("SuperShop-MySQL"),
                    options =>
                    {
                        options.MigrationsAssembly("SuperShop.Web");
                        options.MigrationsHistoryTable("_MyMigrationsHistory");
                    });
            });


        services.AddDbContext<DataContextSQLite>(
            cfg =>
            {
                cfg.UseSqlite(
                    Configuration.GetConnectionString("SuperShop-SQLite"),
                    options =>
                    {
                        options.MigrationsAssembly("SuperShop.Web");
                        options.MigrationsHistoryTable("_MyMigrationsHistory");
                    });
            });


        services.AddMvcCore();
        services.AddRazorPages();


        services
            .AddAuthentication("CookieAuth")
            .AddCookie("CookieAuth",
                config =>
                {
                    config.Cookie.Name = "SuperShop.Cookie";

                    config.LoginPath = "/Home/Authenticate";
                    config.AccessDeniedPath = "/Home/Authenticate";

                    // config.LoginPath = "/Home/Authenticate";
                    // config.AccessDeniedPath = "/Home/Authenticate";
                });

        services.ConfigureApplicationCookie(options =>
        {
            options.LoginPath = "/Account/NotAuthorized";
            options.AccessDeniedPath = "/Account/NotAuthorized";
        });


        //
        // Seeding databases
        services.AddTransient<SeedDb>();

        //
        // injecting repositories helpers
        services.AddScoped<IUserHelper, UserHelper>();
        services.AddScoped<IImageHelper, ImageHelper>();
        services.AddScoped<IStorageHelper, StorageHelper>();
        services.AddScoped<IConverterHelper, ConverterHelper>();

        //
        // injecting mock repositories
        //services.AddScoped<IRepository, Repository>();
        //services.AddScoped<IRepository, MockRepository>();

        //
        // injecting real repositories
        services.AddScoped<IProductsRepository, ProductRepository>();
        services.AddScoped<IOrderRepository, OrderRepository>();

        //
        // injecting cloud repositories
        // services.AddScoped<ICountryRepository, CountryRepository>();
        services.AddScoped<GcpConfigOptions>();
        services.AddScoped<AWSConfigOptions>();
        services.AddScoped<ICloudStorageService, CloudStorageService>();


        services.AddControllersWithViews();
    }


    // This method gets called by the runtime.
    // Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseDeveloperExceptionPage();


        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseExceptionHandler("/Home/Error");
            // The default HSTS value is 30 days.
            // You may want to change this for production scenarios,
            // see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseStatusCodePagesWithReExecute("/error/{0}");

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthentication(); // Must be before UseAuthorization
        app.UseAuthorization();


        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllerRoute(
                "default",
                "{controller=Home}/{action=Index}/{id?}");
        });
    }
}