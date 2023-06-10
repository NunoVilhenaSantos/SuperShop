using Microsoft.AspNetCore.Hosting;
using SuperShop.Web.Areas.Identity;

[assembly: HostingStartup(typeof(IdentityHostingStartup))]

namespace SuperShop.Web.Areas.Identity;

public class IdentityHostingStartup : IHostingStartup
{
    public void Configure(IWebHostBuilder builder)
    {
        builder.ConfigureServices((context, services) => { });
    }
}