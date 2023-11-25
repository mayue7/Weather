using AspNetCoreRateLimit;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

public class Program
{
    public static async Task Main(string[] args)
    {
        IHost webHost = CreateHostBuilder(args).Build();

        using (var scope = webHost.Services.CreateScope())
        {
            // get the ClientPolicyStore instance
            var clientPolicyStore = scope.ServiceProvider.GetRequiredService<IClientPolicyStore>();

            // seed client data from appsettings
            await clientPolicyStore.SeedAsync();
        }

        await webHost.RunAsync();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            });
}
