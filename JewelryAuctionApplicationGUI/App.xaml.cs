
using JewelryAuctionApplicationDAL.Repositories;
using JewelryAuctionApplicationBLL.Stores;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using System.Windows;
using JewelryAuctionApplicationGUI.Navigation;
using JewelryAuctionApplicationBLL.Services;
using JewelryAuctionApplicationGUI.ViewModels;
using JewelryAuctionApplicationDAL;

namespace JewelryAuctionApplicationGUI;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
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
        ConfigureServices(services);
        _serviceProvider = services.BuildServiceProvider();
    }

    private void ConfigureServices(IServiceCollection services)

    {
        services.AddSingleton<AccountStore>();
        services.AddSingleton<NavigationStore>();
        services.AddSingleton<ModalNavigationStore>();
        services.AddSingleton<INavigationService>(s => CreateHomeNavigationService(s)); //when first registered, pass the home view model to the navigation store
        services.AddSingleton<CloseModalNavigationService>();

        services.AddScoped<IPasswordHasher, PasswordHasher>();
        services.AddScoped<IAccountService, AccountService>();
        services.AddScoped<IAccountRepository, AccountRepository>();

        services.AddTransient(s => new HomeViewModel(CreateLoginNavigationService(s)));
        services.AddTransient(CreateLoginViewModel);
        services.AddTransient(CreateNavigationBarViewModel);
        services.AddTransient(CreateSignupViewModel);


        var connectionString = Configuration.GetConnectionString("JewelryAuctionDatabase");
        services.AddDbContext<JewelryAuctionContext>(options =>
        {
            options.UseSqlServer(connectionString);
        }, ServiceLifetime.Scoped);


        services.AddSingleton<MainViewModel>();
        services.AddSingleton(s => new MainWindow()
        {
            DataContext = s.GetRequiredService<MainViewModel>()
        });
    }
    private NavigationBarViewModel CreateNavigationBarViewModel(IServiceProvider serviceProvider)
    {
        return new NavigationBarViewModel(serviceProvider.GetRequiredService<AccountStore>(),
            CreateHomeNavigationService(serviceProvider),          
            CreateLoginNavigationService(serviceProvider),
            CreateSignupNavigationService(serviceProvider));
    }
    private INavigationService CreateHomeNavigationService(IServiceProvider serviceProvider)
    {
        return new LayoutNavigationService<HomeViewModel>(
            serviceProvider.GetRequiredService<NavigationStore>(),
            () => serviceProvider.GetRequiredService<NavigationBarViewModel>(),
            () => serviceProvider.GetRequiredService<HomeViewModel>());
    }
    private INavigationService CreateLoginNavigationService(IServiceProvider serviceProvider)
    {
        return new ModalNavigationService<LoginViewModel>(
            serviceProvider.GetRequiredService<ModalNavigationStore>(),
            () => serviceProvider.GetRequiredService<LoginViewModel>());
    }
    private LoginViewModel CreateLoginViewModel(IServiceProvider serviceProvider)
    {
        CompositeNavigationService navigationService = new(
            serviceProvider.GetRequiredService<CloseModalNavigationService>(),
            CreateHomeNavigationService(serviceProvider));

        return new LoginViewModel(
            serviceProvider.GetRequiredService<AccountStore>(),
            navigationService, serviceProvider.GetRequiredService<IAccountService>(),
            serviceProvider.GetRequiredService<CloseModalNavigationService>());
    }
    private SignupViewModel CreateSignupViewModel(IServiceProvider serviceProvider)
    {
        CompositeNavigationService navigationService = new(
            serviceProvider.GetRequiredService<CloseModalNavigationService>(),
            CreateHomeNavigationService(serviceProvider)); //implement move onto success view

        return new SignupViewModel(
            serviceProvider.GetRequiredService<AccountStore>(),
            navigationService, serviceProvider.GetRequiredService<IAccountService>(),
            serviceProvider.GetRequiredService<CloseModalNavigationService>());
    }
    private INavigationService CreateSignupNavigationService(IServiceProvider serviceProvider)
    {
        return new ModalNavigationService<SignupViewModel>(
            serviceProvider.GetRequiredService<ModalNavigationStore>(),
            () => serviceProvider.GetRequiredService<SignupViewModel>());
    }
    protected override void OnStartup(StartupEventArgs e)
    {
        /*var context = _serviceProvider.GetRequiredService<JewelryAuctionContext>();
        context.Database.Migrate();  */     
        INavigationService initialNavigationService = _serviceProvider.GetRequiredService<INavigationService>(); //first call of navigation, which lead to home page
        initialNavigationService.Navigate();

        MainWindow = _serviceProvider.GetRequiredService<MainWindow>();
        MainWindow.Show();
        base.OnStartup(e);
    }
}
