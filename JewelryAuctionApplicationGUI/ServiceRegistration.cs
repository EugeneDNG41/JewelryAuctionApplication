using JewelryAuctionApplicationBLL.Services;
using JewelryAuctionApplicationBLL.Stores;
using JewelryAuctionApplicationDAL.Repositories;
using JewelryAuctionApplicationGUI.Navigation;
using JewelryAuctionApplicationGUI.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using JewelryAuctionApplicationDAL.Context;

namespace JewelryAuctionApplicationGUI;

public class ServiceRegistration
{
    public void ConfigureServices(IServiceCollection services, IConfiguration Configuration)

    {
        services.AddSingleton<AccountStore>();
        services.AddSingleton<NavigationStore>();
        services.AddSingleton<ModalNavigationStore>();
        services.AddSingleton<INavigationService>(s => CreateHomeNavigationService(s)); //when first registered, pass the home view model to the navigation store
        services.AddSingleton<CloseModalNavigationService>();

        services.AddScoped<IPasswordHasher, PasswordHasher>();
        services.AddScoped<IAccountRepository, AccountRepository>();
        services.AddScoped<IJewelryRepository, JewelryRepository>();
        services.AddScoped<IBidRepository, BidRepository>();
        services.AddScoped<IAuctionRepository, AuctionRepository>();
        services.AddScoped<IAccountService, AccountService>();       
        services.AddScoped<IJewelryService, JewelryService>();
        services.AddScoped<IBidService, BidService>();
        services.AddScoped<IAuctionService, AuctionService>();
        
        

        services.AddTransient(CreateHomeViewModel);
        services.AddTransient(CreateLoginViewModel);
        services.AddTransient(CreateNavigationBarViewModel);
        services.AddTransient(CreateSignupViewModel);
        services.AddTransient(CreateAddJewelryViewModel);
        services.AddTransient(CreateAddAuctionViewModel);


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
    private HomeViewModel CreateHomeViewModel(IServiceProvider serviceProvider)
    {
        return new HomeViewModel(serviceProvider.GetRequiredService<IJewelryService>(),
            CreateJewelryPageNavigationService(serviceProvider),
            serviceProvider.GetRequiredService<IAuctionService>(),
            serviceProvider.GetRequiredService<IBidService>());
    }
    private ParameterNavigationService<JewelryListingViewModel, JewelryPageViewModel> CreateJewelryPageNavigationService(IServiceProvider serviceProvider)
    {
        return new ParameterNavigationService<JewelryListingViewModel, JewelryPageViewModel>(
            serviceProvider.GetRequiredService<NavigationStore>(), null,
            (parameter) => new JewelryPageViewModel(parameter, 
            serviceProvider.GetRequiredService<IBidService>(), 
            () => serviceProvider.GetRequiredService<NavigationBarViewModel>(),
            CreateAddBidNavigationService(serviceProvider),
            CreateLoginNavigationService(serviceProvider),
            serviceProvider.GetRequiredService<AccountStore>()));
    }
    private ParameterNavigationService<JewelryListingViewModel, AddBidViewModel> CreateAddBidNavigationService(IServiceProvider serviceProvider)
    {
        return new ParameterNavigationService<JewelryListingViewModel, AddBidViewModel>(
             null, serviceProvider.GetRequiredService<ModalNavigationStore>(),
             (parameter) => new AddBidViewModel(parameter, serviceProvider.GetRequiredService<IBidService>(),
             serviceProvider.GetRequiredService<CloseModalNavigationService>(),
             serviceProvider.GetRequiredService<AccountStore>()));
    }
    private AddJewelryViewModel CreateAddJewelryViewModel(IServiceProvider serviceProvider)
    {
        return new AddJewelryViewModel(serviceProvider.GetRequiredService<IJewelryService>(),
            serviceProvider.GetRequiredService<CloseModalNavigationService>());
    }
    private AddAuctionViewModel CreateAddAuctionViewModel(IServiceProvider serviceProvider)
    {
        return new AddAuctionViewModel(serviceProvider.GetRequiredService<IAuctionService>(),
            serviceProvider.GetRequiredService<IJewelryService>(),
            serviceProvider.GetRequiredService<CloseModalNavigationService>());
    }
    private NavigationBarViewModel CreateNavigationBarViewModel(IServiceProvider serviceProvider)
    {
        return new NavigationBarViewModel(serviceProvider.GetRequiredService<AccountStore>(),
            CreateHomeNavigationService(serviceProvider),
            CreateLoginNavigationService(serviceProvider),
            CreateSignupNavigationService(serviceProvider),
            CreateAddJewelryNavigationService(serviceProvider),
            CreateAddAuctionNavigationService(serviceProvider));
    }
    private INavigationService CreateAddJewelryNavigationService(IServiceProvider serviceProvider)
    {
        return new ModalNavigationService<AddJewelryViewModel>(
            serviceProvider.GetRequiredService<ModalNavigationStore>(),
            () => serviceProvider.GetRequiredService<AddJewelryViewModel>());
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
    private INavigationService CreateAddAuctionNavigationService(IServiceProvider serviceProvider)
    {
        return new ModalNavigationService<AddAuctionViewModel>(
            serviceProvider.GetRequiredService<ModalNavigationStore>(),
            () => serviceProvider.GetRequiredService<AddAuctionViewModel>());
    }
    private LoginViewModel CreateLoginViewModel(IServiceProvider serviceProvider)
    {
        CompositeNavigationService navigationService = new(
            serviceProvider.GetRequiredService<CloseModalNavigationService>()/*,
            CreateHomeNavigationService(serviceProvider)*/);

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
}
