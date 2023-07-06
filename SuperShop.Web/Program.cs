using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SuperShop.Web.Data;

namespace SuperShop.Web;

public class Program
{
    public static void Main(string[] args)
    {
        var portHttp = GetAvailablePort();
        var urlHttp = $"http://localhost:{portHttp}";

        var portHttps = GetAvailablePort();
        var urlHttps =
            $"https://localhost:{portHttps}";

        var urls =
            $"{urlHttps};{urlHttp}";

        // CreateHostBuilder(args).Build().Run();

        // var host = CreateHostBuilder(args).Build();
        var host =
            CreateHostBuilder(
                    args, urlHttp, urlHttps, portHttp, portHttps, urls)
                .Build();

        RunSeeding(host);

        // OpenBrowser(urlHttp);
        OpenBrowser(urlHttps);

        // host.Run();

        host.RunAsync().Wait();
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


    private static IHostBuilder CreateHostBuilder(
        string[] args,
        string urlHttp, string urlHttps,
        int portHttp, int portHttps, string urls)
    {
        return Host.CreateDefaultBuilder(args).ConfigureWebHostDefaults(
            webBuilder =>
            {
                webBuilder.UseStartup<Startup>();

                //
                // O uso de ":0"
                // indica que o Kestrel deve selecionar
                // uma porta disponível automaticamente
                //

                // webBuilder.ConfigureKestrel((context, options) =>
                // {
                //     // var port = GetAvailablePort();
                //     webBuilder.UseUrls(url);
                //     options.ListenLocalhost(port);
                // });
                // webBuilder.UseUrls(url);

                // webBuilder.ConfigureKestrel(options =>
                // {
                //     webBuilder.UseUrls(urls);
                //     options.ListenLocalhost(portHttp);
                // });


                // webBuilder.ConfigureKestrel(options =>
                // {
                //     options.ListenLocalhost(portHttps,
                //         builder => { builder.UseHttps(); });
                // });


                webBuilder.ConfigureKestrel((context, options) =>
                {
                    options.ListenAnyIP(portHttps, listenOptions =>
                    {
                        listenOptions.Protocols = HttpProtocols.Http1;
                        listenOptions.Protocols = HttpProtocols.Http2;
                        // listenOptions.Protocols =
                        //     HttpProtocols.Http1AndHttp2AndHttp3;
                        listenOptions.UseHttps();
                    });
                });

                // webBuilder.UseQuic();
                webBuilder.UseUrls(urls);
            });
    }


    private static int GetAvailablePort()
    {
        var listener =
            new TcpListener(
                IPAddress.Loopback, 0);
        try
        {
            listener.Start();
            return ((IPEndPoint) listener.LocalEndpoint).Port;
        }
        finally
        {
            listener.Stop();
        }
    }


    private static void OpenBrowser(string url)
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            Process.Start(new ProcessStartInfo
            {
                FileName = url,
                UseShellExecute = true
            });

        else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            Process.Start("xdg-open", url);

        else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            Process.Start("open", url);
    }
}