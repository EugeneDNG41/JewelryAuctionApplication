
using JewelryAuctionApplicationDAL.Repositories;
using JewelryAuctionApplicationBLL.Stores;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using System.Windows;
using JewelryAuctionApplicationGUI.Navigation;
using Microsoft.Extensions.Hosting;
using JewelryAuctionApplicationBLL.Services;

namespace JewelryAuctionApplicationGUI;

public partial class App : Application
{
    private readonly IHost _host;

    public App()
    {
        _host = Host.CreateDefaultBuilder()
            .ConfigureAppConfiguration((context, builder) =>
            {
                builder.SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            })
            .ConfigureServices((context, services) =>
            {
                var configuration = context.Configuration;
                new ServiceRegistration().ConfigureServices(services, configuration);
                services.AddHostedService<AuctionCheckService>(); // Ensure hosted service is added
            })
            .Build();
    }

    protected override async void OnStartup(StartupEventArgs e)
    {
        await _host.StartAsync();
        INavigationService initialNavigationService = _host.Services.GetRequiredService<INavigationService>(); //first call of navigation, which lead to home page
        initialNavigationService.Navigate();

        MainWindow = _host.Services.GetRequiredService<MainWindow>();
        MainWindow.Show();
        base.OnStartup(e);
    }
    protected override async void OnExit(ExitEventArgs e)
    {
        await _host.StopAsync();
        _host.Dispose();
        base.OnExit(e);
    }
}
