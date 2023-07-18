using System;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
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
                    // Email settings.
                    cfg.User.RequireUniqueEmail = true;

                    // Password settings.
                    cfg.Password.RequireDigit = false;
                    cfg.Password.RequiredLength = 6;
                    cfg.Password.RequiredUniqueChars = 0;
                    cfg.Password.RequireUppercase = false;
                    cfg.Password.RequireLowercase = false;
                    cfg.Password.RequireNonAlphanumeric = false;

                    // SignIn settings.
                    cfg.SignIn.RequireConfirmedEmail = false;
                    cfg.SignIn.RequireConfirmedAccount = false;
                    cfg.SignIn.RequireConfirmedPhoneNumber = false;

                    // Lockout settings.
                })
            .AddEntityFrameworkStores<DataContextMsSql>()
            .AddEntityFrameworkStores<DataContextMySql>()
            .AddEntityFrameworkStores<DataContextSqLite>();


        // se estiverem ambos os AddIdentity e AddDefaultIdentity
        // o AddIdentity sobrepõe o AddDefaultIdentity
        // mas a aplicação não corre
        //
        // já esta no AddIdentity
        //
        // senão adicionar aqui
        //
        // services.AddDefaultIdentity<IdentityUser>(
        //         options =>
        //         {
        //             options.SignIn.RequireConfirmedAccount = false;
        //
        //             // options.SignIn.RequireConfirmedAccount = true;
        //         })
        //     .AddEntityFrameworkStores<DataContextMsSql>()
        //     .AddEntityFrameworkStores<DataContextMySql>()
        //     .AddEntityFrameworkStores<DataContextSqLite>();


        // PRODUCTION
        // Services for production.
        // services.Configure<IdentityOptions>(options =>
        // {
        //     // Password settings.
        //     options.Password.RequireDigit = true;
        //     options.Password.RequireLowercase = true;
        //     options.Password.RequireNonAlphanumeric = true;
        //     options.Password.RequireUppercase = true;
        //     options.Password.RequiredLength = 8;
        //     options.Password.RequiredUniqueChars = 1;
        //
        //     // Lockout settings.
        //     options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
        //     options.Lockout.MaxFailedAccessAttempts = 5;
        //     options.Lockout.AllowedForNewUsers = true;
        //
        //     // User settings.
        //     options.User.AllowedUserNameCharacters =
        //         "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
        //     options.User.RequireUniqueEmail = false;
        // });


        services.AddDbContext<DataContextMsSql>(
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


        services.AddDbContext<DataContextMySql>(
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


        services.AddDbContext<DataContextSqLite>(
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


        // Configurando a autenticação JWT
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters =
                    new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,

                        ValidIssuer = Configuration["Tokens:Issuer"],
                        ValidAudience = Configuration["Tokens:Audience"],

                        IssuerSigningKey =
                            new SymmetricSecurityKey(
                                Encoding.UTF8.GetBytes(
                                    Configuration["Tokens:Key"]))
                    };
            });


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


        // criado por nos, para configurar o tempo de expiração do cookie
        // e redirecionar para a página de login
        services.ConfigureApplicationCookie(options =>
        {
            options.LoginPath = "/Account/NotAuthorized";
            options.AccessDeniedPath = "/Account/NotAuthorized";
        });


        services.ConfigureApplicationCookie(options =>
        {
            // Cookie settings
            options.Cookie.HttpOnly = true;
            options.ExpireTimeSpan = TimeSpan.FromMinutes(5);

            options.LoginPath = "/Identity/Account/Login";
            options.AccessDeniedPath = "/Identity/Account/AccessDenied";
            options.SlidingExpiration = true;
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
        services.AddScoped<ICountryRepository, CountryRepository>();

        //
        // injecting cloud repositories
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

            // app.UseExceptionHandler("/Home/Error");
        }
        else
        {
            // app.UseExceptionHandler("/Home/Error");
            app.UseExceptionHandler("/Errors/Error");

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