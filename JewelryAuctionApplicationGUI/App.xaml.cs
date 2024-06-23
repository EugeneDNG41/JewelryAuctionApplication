
using JewelryAuctionApplicationDAL.Repositories;
using JewelryAuctionApplicationBLL.Stores;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using System.Windows;
using JewelryAuctionApplicationGUI.Navigation;

namespace JewelryAuctionApplicationGUI;

public partial class App : Application
{
    private readonly ServiceProvider _serviceProvider;   
    private IConfiguration Configuration {  get; }
    public App()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
        Configuration = builder.Build();
       
        IServiceCollection services = new ServiceCollection();
        new ServiceRegistration().ConfigureServices(services, Configuration);
        _serviceProvider = services.BuildServiceProvider();
    }

    protected override void OnStartup(StartupEventArgs e)
    { 
        INavigationService initialNavigationService = _serviceProvider.GetRequiredService<INavigationService>(); //first call of navigation, which lead to home page
        initialNavigationService.Navigate();

        MainWindow = _serviceProvider.GetRequiredService<MainWindow>();
        MainWindow.Show();
        base.OnStartup(e);
    }
}
