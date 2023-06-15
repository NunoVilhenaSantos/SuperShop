using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SuperShop.Web.Data;
using SuperShop.Web.Data.Entity;
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
            }).AddEntityFrameworkStores<DataContext>();


        // este é o por defeito, mas já existe o de cima
        //services.AddDefaultIdentity<IdentityUser>(
        //        options =>
        //            options.SignIn.RequireConfirmedAccount = true)
        //    .AddEntityFrameworkStores<DataContext>();


        services.AddDbContext<DataContext>(
            cfg =>
            {
                //cfg.UseSqlServer(
                //    Configuration.GetConnectionString(
                //        "SomeeConnection"),
                //        options => options.EnableRetryOnFailure());

                cfg.UseSqlServer(
                    Configuration.GetConnectionString(
                        "AzureConnectionNuno"),
                    options => options.EnableRetryOnFailure());
            });


        // services.AddDbContext<DataContext>(
        //     cfg =>
        //     {
        //         cfg.UseSqlServer(
        //             Configuration.GetConnectionString(
        //                 "AzureConnectionNuno"),
        //             options => options.EnableRetryOnFailure());
        //     });


        // services.AddDbContext<DataContext>(
        //     cfg =>
        //     {
        //         cfg.UseSqlServer(
        //             Configuration.GetConnectionString(
        //                 "AzureConnectionRuben"),
        //             options => options.EnableRetryOnFailure());
        //     });


        services.AddMvcCore();
        services.AddRazorPages();


        services
            .AddAuthentication("CookieAuth")
            .AddCookie("CookieAuth",
                config =>
                {
                    config.Cookie.Name = "SuperShop.Cookie";

                    // config.LoginPath = "/Home/Authenticate";
                    // config.AccessDeniedPath = "/Home/Authenticate";

                    config.LoginPath = "/Home/Authenticate";
                    config.AccessDeniedPath = "/Home/Authenticate";

                    config.LoginPath = "/Account/NotAuthorized";
                    config.AccessDeniedPath = "/Account/NotAuthorized";
                });


        services.AddTransient<SeedDb>();

        services.AddScoped<IUserHelper, UserHelper>();
        services.AddScoped<IImageHelper, ImageHelper>();
        services.AddScoped<IStorageHelper, StorageHelper>();
        services.AddScoped<IConverterHelper, ConverterHelper>();

        //services.AddScoped<IRepository, Repository>();
        //services.AddScoped<IRepository, MockRepository>();

        services.AddScoped<IProductsRepository, ProductRepository>();

        //services.AddScoped<ICountryRepository, CountryRepository>();
        services.AddScoped<GCPConfigOptions>();
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

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthentication(); // Must be before UseAuthorization
        app.UseAuthorization();

        // app.MapRazorPages();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllerRoute(
                "default",
                "{controller=Home}/{action=Index}/{id?}");
        });
    }
}