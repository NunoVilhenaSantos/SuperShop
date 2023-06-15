using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SuperShop.Web.Data;

namespace SuperShop.Web;

public class Program
{
    public static void Main(string[] args)
    {
        // CreateHostBuilder(args).Build().Run();
        var host = CreateHostBuilder(args).Build();
        RunSeeding(host);
        host.Run();
    }


    private static void RunSeeding(IHost host)
    {
        var scopeFactory = host.Services.GetService<IServiceScopeFactory>();
        // var scopeFactory = host.Services.GetService(typeof(IServiceScopeFactory)) as IServiceScopeFactory;
        // var scopeFactory = host.Services.GetService(typeof(IServiceProvider)) as IServiceProvider;

        using var scope = scopeFactory.CreateScope();
        var seeder = scope.ServiceProvider.GetService<SeedDb>();
        seeder.SeedAsync().Wait();
    }

    // private static IHostBuilder CreateHostBuilder(string[] args) =>
    //     Host.CreateDefaultBuilder(args)
    //         .ConfigureWebHostDefaults(webBuilder =>
    //         {
    //             webBuilder.UseStartup<Startup>()
    //                 .ConfigureLogging(logging =>
    //                 {
    //                     logging.AddAzureWebAppDiagnostics();
    //
    //                     logging.AddBlobServiceClient<>(options =>
    //                     {
    //                         options.SasToken = "<your_sas_token>";
    //                     });
    //
    //                     logging.AddAzureBlobStorage(options =>
    //                     {
    //                         options.SasToken = "<your_sas_token>";
    //                     });
    //                 });
    //         });

    private static IHostBuilder CreateHostBuilder(string[] args)
    {
        return Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            });
    }
}