using System;
using System.Threading.Tasks;
using Azure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

// using Microsoft.Azure.Management.Sql;
// using Microsoft.Azure.Management.Sql.Models;
using Azure.Identity;
using Azure.ResourceManager;
using Azure.ResourceManager.Sql;
using Azure.ResourceManager.Sql.Models;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SuperShop.Web.Data;


namespace SuperShop.Web;

public class Program
{
    public static void Main(string[] args)
    {
        // CreateFirewallRuleAsync().Wait();

        // CreateHostBuilder(args).Build().Run();
        var host = CreateHostBuilder(args).Build();
        RunSeeding(host);
        host.Run();
    }


    // Replace the placeholder values in the CreateFirewallRule
    // method with your actual values,
    // such as your IP address, Azure subscription ID, resource group name,
    // SQL server name, Azure AD client ID, client secret, and tenant ID.

    // By running this code, it will create a firewall rule in
    // Azure SQL Server to allow access from the specified IP address.
    //
    // Make sure you have the necessary permissions and credentials
    // to modify the firewall rules in your Azure environment.
    // private static async Task CreateFirewallRuleAsync()
    // {
    //     var ipAddress = "188.140.5.62"; // Replace with your IP address
    //     var subscriptionId =
    //         "YourSubscriptionId"; // Replace with your Azure subscription ID
    //     var resourceGroupName =
    //         "YourResourceGroupName"; // Replace with the resource group name where your SQL server is located
    //     var serverName =
    //         "supershopdbservernuno"; // Replace with your SQL server name
    //
    //     var credential = new DefaultAzureCredential();
    //     var armClient = new ArmClient(credential);
    //
    //     var sqlManagementClient =
    //         new SqlFirewallRuleResource(armClient, credential);
    //
    //     var firewallRuleName = $"Allow IP {ipAddress}";
    //
    //     var firewallRuleParams =
    //         new SqlFirewallRuleResource().UpdateAsync(WaitUntil.Completed,
    //             default, default);
    //
    //     var response =
    //         await sqlManagementClient.AddFileServiceClient()..CreateOrUpdateAsync(
    //             resourceGroupName, serverName, firewallRuleName,
    //             firewallRuleParams);
    //
    //     Console.WriteLine(response != null
    //         ? "Firewall rule created successfully."
    //         : "Error creating firewall rule.");
    // }


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