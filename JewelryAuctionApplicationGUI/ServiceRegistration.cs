using JewelryAuctionApplicationBLL.Services;
using JewelryAuctionApplicationBLL.Stores;
using JewelryAuctionApplicationDAL.Repositories;
using JewelryAuctionApplicationGUI.Navigation;
using JewelryAuctionApplicationGUI.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using JewelryAuctionApplicationDAL.Context;
using JewelryAuctionApplicationDAL.Models;

namespace JewelryAuctionApplicationGUI;

public class ServiceRegistration
{
    public void ConfigureServices(IServiceCollection services, IConfiguration Configuration)
    {
        // Add singleton services
        services.AddSingleton<AccountStore>();
        services.AddSingleton<NavigationStore>();
        services.AddSingleton<ModalNavigationStore>();
        services.AddSingleton<INavigationService>(s => CreateHomeNavigationService(s));
        services.AddSingleton<CloseModalNavigationService>();

        // Add scoped services (per request)
        services.AddScoped<IPasswordHasher, PasswordHasher>();
        services.AddScoped<IAccountRepository, AccountRepository>();
        services.AddScoped<IJewelryRepository, JewelryRepository>();
        services.AddScoped<IBidRepository, BidRepository>();
        services.AddScoped<IAuctionRepository, AuctionRepository>();
        services.AddScoped<IAccountService, AccountService>();
        services.AddScoped<IJewelryService, JewelryService>();
        services.AddScoped<IBidService, BidService>();
        services.AddScoped<IAuctionService, AuctionService>();

        // Add transient services
        services.AddTransient(CreateHomeViewModel);
        services.AddTransient(CreateLoginViewModel);
        services.AddTransient(CreateNavigationBarViewModel);
        services.AddTransient(CreateSignupViewModel);
        services.AddTransient(CreateAddJewelryViewModel);
        services.AddTransient(CreatePastAuctionsViewModel);
        services.AddTransient(CreateAddCreditViewModel);
        services.AddTransient(CreateAccountManagementViewModel);
        services.AddTransient(CreateCreateAccountViewModel);
        services.AddTransient(CreateProfileViewModel);
        services.AddTransient(CreateChangePasswordViewModel);
        services.AddTransient(CreateJewelryManagementViewModel);

        // Add DbContext with scoped lifetime
        var connectionString = Configuration.GetConnectionString("JewelryAuctionDatabase");
        services.AddDbContext<JewelryAuctionContext>(options =>
        {
            options.UseSqlServer(connectionString);
        }, ServiceLifetime.Scoped);
        services.AddHostedService<AuctionCheckService>();
        // Singleton MainViewModel and MainWindow (assuming these are UI-related)
        services.AddSingleton<MainViewModel>();
        services.AddSingleton(s => new MainWindow()
        {
            DataContext = s.GetRequiredService<MainViewModel>()
        });
    }
    private HomeViewModel CreateHomeViewModel(IServiceProvider serviceProvider)
    {
        return new HomeViewModel(serviceProvider.GetRequiredService<IJewelryService>(),
            CreateJewelryPageNavigationService(serviceProvider));
    }
    private PastAuctionsViewModel CreatePastAuctionsViewModel(IServiceProvider serviceProvider)
    {
        return new PastAuctionsViewModel(serviceProvider.GetRequiredService<IJewelryService>(),
            CreateJewelryPageNavigationService(serviceProvider));
    }
    private ParameterNavigationService<JewelryListingViewModel, JewelryPageViewModel> CreateJewelryPageNavigationService(IServiceProvider serviceProvider)
    {
        return new ParameterNavigationService<JewelryListingViewModel, JewelryPageViewModel>(
            serviceProvider.GetRequiredService<NavigationStore>(), null,
            (parameter) => new JewelryPageViewModel(parameter, 
            serviceProvider.GetRequiredService<IAccountService>(), 
            () => serviceProvider.GetRequiredService<NavigationBarViewModel>(),
            CreateAddBidNavigationService(serviceProvider),
            CreateLoginNavigationService(serviceProvider),
            serviceProvider.GetRequiredService<AccountStore>()));
    }
    private ParameterNavigationService<JewelryListingViewModel, AddBidViewModel> CreateAddBidNavigationService(IServiceProvider serviceProvider)
    {
        return new ParameterNavigationService<JewelryListingViewModel, AddBidViewModel>(
            null, serviceProvider.GetRequiredService<ModalNavigationStore>(),
            (parameter) => new AddBidViewModel(parameter,
            serviceProvider.GetRequiredService<IAuctionService>(),
            serviceProvider.GetRequiredService<IBidService>(),
            serviceProvider.GetRequiredService<CloseModalNavigationService>(),
            CreateAddCreditNavigationService(serviceProvider),
            serviceProvider.GetRequiredService<AccountStore>()));
    }
    private ParameterNavigationService<Account, UpdateAccountViewModel> CreateUpdateAccountNavigationService(IServiceProvider serviceProvider)
    {
        var navigationService = new CompositeNavigationService(
            serviceProvider.GetRequiredService<CloseModalNavigationService>(),
            CreateAccountManagementNavigationService(serviceProvider));

        return new ParameterNavigationService<Account, UpdateAccountViewModel>(
            null, serviceProvider.GetRequiredService<ModalNavigationStore>(),
            (parameter) => new UpdateAccountViewModel(parameter,
            serviceProvider.GetRequiredService<IAccountService>(),
            serviceProvider.GetRequiredService<IBidService>(),
            serviceProvider.GetRequiredService<CloseModalNavigationService>(),
            navigationService));
    }
    private ParameterNavigationService<Jewelry, AddAuctionViewModel> CreateAddAuctionNavigationService(IServiceProvider serviceProvider)
    {
        var navigationService = new CompositeNavigationService(
           serviceProvider.GetRequiredService<CloseModalNavigationService>(),
           CreateJewelryManagementNavigationService(serviceProvider));

        return new ParameterNavigationService<Jewelry, AddAuctionViewModel>(
            null, serviceProvider.GetRequiredService<ModalNavigationStore>(),
            (parameter) => new AddAuctionViewModel(parameter,
            serviceProvider.GetRequiredService<IAuctionService>(),
            serviceProvider.GetRequiredService<IJewelryService>(),
            serviceProvider.GetRequiredService<CloseModalNavigationService>(),
            navigationService));
    }
    private ParameterNavigationService<JewelryListingViewModel, UpdateJewelryViewModel> CreateUpdateJewelryNavigationService(IServiceProvider serviceProvider)
    {
        var navigationService = new CompositeNavigationService(
           serviceProvider.GetRequiredService<CloseModalNavigationService>(),
           CreateJewelryManagementNavigationService(serviceProvider));

        return new ParameterNavigationService<JewelryListingViewModel, UpdateJewelryViewModel>(
            null, serviceProvider.GetRequiredService<ModalNavigationStore>(),
            (parameter) => new UpdateJewelryViewModel(parameter,
            serviceProvider.GetRequiredService<IJewelryService>(),
            serviceProvider.GetRequiredService<CloseModalNavigationService>(),
            navigationService));
    }
    private AddJewelryViewModel CreateAddJewelryViewModel(IServiceProvider serviceProvider)
    {
        var navigationService = new CompositeNavigationService(
           serviceProvider.GetRequiredService<CloseModalNavigationService>(),
           CreateJewelryManagementNavigationService(serviceProvider));

        return new AddJewelryViewModel(serviceProvider.GetRequiredService<IJewelryService>(),
            serviceProvider.GetRequiredService<CloseModalNavigationService>(), navigationService);
    }
    private AddCreditViewModel CreateAddCreditViewModel(IServiceProvider serviceProvider)
    {
        return new AddCreditViewModel(serviceProvider.GetRequiredService<AccountStore>(),
               serviceProvider.GetRequiredService<CloseModalNavigationService>(),
               serviceProvider.GetRequiredService<IAccountService>(),
               serviceProvider.GetRequiredService<IBidService>());
    }
    private ProfileViewModel CreateProfileViewModel(IServiceProvider serviceProvider)
    {
        return new ProfileViewModel(serviceProvider.GetRequiredService<AccountStore>(),
            serviceProvider.GetRequiredService<IAccountService>(),
            CreateChangePasswordNavigationService(serviceProvider));
    }
    private ChangePasswordViewModel CreateChangePasswordViewModel(IServiceProvider serviceProvider)
    {
        return new ChangePasswordViewModel(serviceProvider.GetRequiredService<IAccountService>(),
            serviceProvider.GetRequiredService<AccountStore>(),
            serviceProvider.GetRequiredService<CloseModalNavigationService>());
    }
    private NavigationBarViewModel CreateNavigationBarViewModel(IServiceProvider serviceProvider)
    {
        return new NavigationBarViewModel(serviceProvider.GetRequiredService<AccountStore>(),
            CreateHomeNavigationService(serviceProvider),
            CreateLoginNavigationService(serviceProvider),
            CreateSignupNavigationService(serviceProvider),
            CreateAccountManagementNavigationService(serviceProvider),
            CreateJewelryManagementNavigationService(serviceProvider),
            CreatePastAuctionsNavigationService(serviceProvider),
            CreateAddCreditNavigationService(serviceProvider),
            CreateProfileNavigationService(serviceProvider));
    }
    public INavigationService CreateProfileNavigationService(IServiceProvider serviceProvider)
    {
        return new LayoutNavigationService<ProfileViewModel>(
            serviceProvider.GetRequiredService<NavigationStore>(),
            () => serviceProvider.GetRequiredService<NavigationBarViewModel>(),
            () => serviceProvider.GetRequiredService<ProfileViewModel>());
    }
    private INavigationService CreateAddJewelryNavigationService(IServiceProvider serviceProvider)
    {
        return new ModalNavigationService<AddJewelryViewModel>(
            serviceProvider.GetRequiredService<ModalNavigationStore>(),
            () => serviceProvider.GetRequiredService<AddJewelryViewModel>());
    }
    private INavigationService CreateChangePasswordNavigationService(IServiceProvider serviceProvider)
    {
        return new ModalNavigationService<ChangePasswordViewModel>(
            serviceProvider.GetRequiredService<ModalNavigationStore>(),
            () => serviceProvider.GetRequiredService<ChangePasswordViewModel>());
    }
    private INavigationService CreateAddCreditNavigationService(IServiceProvider serviceProvider)
    {
        return new ModalNavigationService<AddCreditViewModel>(
               serviceProvider.GetRequiredService<ModalNavigationStore>(),
               () => serviceProvider.GetRequiredService<AddCreditViewModel>());
    }
    private INavigationService CreateHomeNavigationService(IServiceProvider serviceProvider)
    {
        return new LayoutNavigationService<HomeViewModel>(
            serviceProvider.GetRequiredService<NavigationStore>(),
            () => serviceProvider.GetRequiredService<NavigationBarViewModel>(),
            () => serviceProvider.GetRequiredService<HomeViewModel>());
    }
    private INavigationService CreatePastAuctionsNavigationService(IServiceProvider serviceProvider)
    {
        return new LayoutNavigationService<PastAuctionsViewModel>(
            serviceProvider.GetRequiredService<NavigationStore>(),
            () => serviceProvider.GetRequiredService<NavigationBarViewModel>(),
            () => serviceProvider.GetRequiredService<PastAuctionsViewModel>());
    }
    private INavigationService CreateLoginNavigationService(IServiceProvider serviceProvider)
    {
        return new ModalNavigationService<LoginViewModel>(
            serviceProvider.GetRequiredService<ModalNavigationStore>(),
            () => serviceProvider.GetRequiredService<LoginViewModel>());
    }
    private INavigationService CreateCreateAccountNavigationService(IServiceProvider serviceProvider)
    {
        return new ModalNavigationService<CreateAccountViewModel>(
             serviceProvider.GetRequiredService<ModalNavigationStore>(),
             () => serviceProvider.GetRequiredService<CreateAccountViewModel>());
    }
    private INavigationService CreateAccountManagementNavigationService(IServiceProvider serviceProvider)
    {
        return new LayoutNavigationService<AccountManagementViewModel>(
               serviceProvider.GetRequiredService<NavigationStore>(),
               () => serviceProvider.GetRequiredService<NavigationBarViewModel>(),
               () => serviceProvider.GetRequiredService<AccountManagementViewModel>());
    }
    private INavigationService CreateJewelryManagementNavigationService(IServiceProvider serviceProvider)
    {
        return new LayoutNavigationService<JewelryManagementViewModel>(
               serviceProvider.GetRequiredService<NavigationStore>(),
               () => serviceProvider.GetRequiredService<NavigationBarViewModel>(),
               () => serviceProvider.GetRequiredService<JewelryManagementViewModel>());
    }
    private LoginViewModel CreateLoginViewModel(IServiceProvider serviceProvider)
    {
        CompositeNavigationService accountManagementNavigationService = new(
            serviceProvider.GetRequiredService<CloseModalNavigationService>(),
            CreateAccountManagementNavigationService(serviceProvider));
        CompositeNavigationService jewelryManagementNavigationService = new(
            serviceProvider.GetRequiredService<CloseModalNavigationService>(),
            CreateJewelryManagementNavigationService(serviceProvider));

        return new LoginViewModel(
            serviceProvider.GetRequiredService<AccountStore>(),
            accountManagementNavigationService, jewelryManagementNavigationService, 
            serviceProvider.GetRequiredService<IAccountService>(),
            serviceProvider.GetRequiredService<CloseModalNavigationService>(),
            CreateSignupNavigationService(serviceProvider));
    }
    private CreateAccountViewModel CreateCreateAccountViewModel(IServiceProvider serviceProvider)
    {
        return new CreateAccountViewModel(serviceProvider.GetRequiredService<IAccountService>(),
            serviceProvider.GetRequiredService<CloseModalNavigationService>());
    }

    private AccountManagementViewModel CreateAccountManagementViewModel(IServiceProvider serviceProvider)
    {
        return new AccountManagementViewModel(serviceProvider.GetRequiredService<AccountStore>(),
            serviceProvider.GetRequiredService<IAccountService>(),
            serviceProvider.GetRequiredService<IBidService>(),
            CreateCreateAccountNavigationService(serviceProvider),
            CreateAccountManagementNavigationService(serviceProvider),
            CreateUpdateAccountNavigationService(serviceProvider));
    }
    private JewelryManagementViewModel CreateJewelryManagementViewModel(IServiceProvider serviceProvider)
    {
        var navigationService = new CompositeNavigationService(
            serviceProvider.GetRequiredService<CloseModalNavigationService>(),
            CreateJewelryManagementNavigationService(serviceProvider));

        return new JewelryManagementViewModel(serviceProvider.GetRequiredService<AccountStore>(),
            serviceProvider.GetRequiredService<IJewelryService>(),
            CreateAddJewelryNavigationService(serviceProvider),
            navigationService, CreateJewelryPageNavigationService(serviceProvider),
            CreateUpdateJewelryNavigationService(serviceProvider),
            CreateAddAuctionNavigationService(serviceProvider));           
    }
    private SignupViewModel CreateSignupViewModel(IServiceProvider serviceProvider)
    {
        CompositeNavigationService navigationService = new(
            serviceProvider.GetRequiredService<CloseModalNavigationService>(),
            CreateHomeNavigationService(serviceProvider));

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
