using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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
                    // User settings.
                    //cfg.User.AllowedUserNameCharacters =
                    //         "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                    cfg.User.RequireUniqueEmail = true;


                    // Password settings.
                    cfg.Password.RequireDigit = false;
                    cfg.Password.RequiredLength = 6;
                    cfg.Password.RequiredUniqueChars = 0;
                    cfg.Password.RequireUppercase = false;
                    cfg.Password.RequireLowercase = false;
                    cfg.Password.RequireNonAlphanumeric = false;


                    // SignIn settings.
                    cfg.SignIn.RequireConfirmedEmail = true;
                    cfg.SignIn.RequireConfirmedAccount = false;
                    cfg.SignIn.RequireConfirmedPhoneNumber = false;

                    // Lockout settings.
                    //     cfg.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                    //     cfg.Lockout.MaxFailedAccessAttempts = 5;
                    //     cfg.Lockout.AllowedForNewUsers = true;
                    //

                    // Token settings.
                    cfg.Tokens.AuthenticatorTokenProvider =
                        TokenOptions.DefaultAuthenticatorProvider;
                })
            .AddDefaultTokenProviders()
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


        //
        // Configurando a autenticação JWT
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddCookie()
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


                options.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        if (context.Exception.GetType() ==
                            typeof(SecurityTokenExpiredException))
                            context.Response.Headers.Add("Token-Expired",
                                "true");

                        return Task.CompletedTask;
                    }
                };


                // Atualize o evento OnAuthenticationFailed
                //options.Events = new JwtBearerEvents
                //{
                //    OnAuthenticationFailed = context =>
                //    {
                //        if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                //        {
                //            // Obtenha a data de expiração do token
                //            var expirationDate = context.SecurityToken.ValidTo;

                //            // Calcule o tempo restante
                //            var timeRemaining = GetTimeRemaining(expirationDate);

                //            // Adicione um cabeçalho personalizado à resposta HTTP
                //            context.Response.Headers.Add("Token-Remaining-Time", timeRemaining.TotalDays.ToString());
                //        }
                //        return Task.CompletedTask;
                //    }
                //};
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
            // Cookie settings
            options.Cookie.HttpOnly = true;

            // ExpireTimeSpan settings
            // options.ExpireTimeSpan = TimeSpan.FromTicks(5);
            // options.ExpireTimeSpan = TimeSpan.FromMilliseconds(5);
            // options.ExpireTimeSpan = TimeSpan.FromSeconds(5);
            // options.ExpireTimeSpan = TimeSpan.FromMinutes(5);
            // options.ExpireTimeSpan = TimeSpan.FromHours(5);
            // options.ExpireTimeSpan = TimeSpan.FromDays(5);

            options.ExpireTimeSpan = TimeSpan.FromDays(15);


            // LoginPath and AccessDeniedPath settings
            options.LoginPath = "/Account/NotAuthorized";
            options.AccessDeniedPath = "/Account/NotAuthorized";

            options.SlidingExpiration = true;
        });


        //services.ConfigureApplicationCookie(options =>
        //{
        //    // Cookie settings
        //    options.Cookie.HttpOnly = true;
        //    options.ExpireTimeSpan = TimeSpan.FromMinutes(5);

        //    options.LoginPath = "/Identity/Account/Login";
        //    options.AccessDeniedPath = "/Identity/Account/AccessDenied";
        //    options.SlidingExpiration = true;
        //});


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
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<ICountryRepository, CountryRepository>();
        services.AddScoped<IProductsRepository, ProductRepository>();

        //
        // injecting cloud repositories
        services.AddScoped<GcpConfigOptions>();
        services.AddScoped<AWSConfigOptions>();
        services.AddScoped<ICloudStorageService, CloudStorageService>();


        services.AddControllersWithViews();
    }


    // Adicione um método para calcular o tempo restante de expiração do token
    private TimeSpan GetTimeRemaining(DateTime expirationDate)
    {
        return expirationDate - DateTime.UtcNow;
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