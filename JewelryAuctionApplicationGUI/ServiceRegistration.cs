﻿using JewelryAuctionApplicationBLL.Services;
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
        services.AddTransient(CreateAddAuctionViewModel);
        services.AddTransient(CreatePastAuctionsViewModel);
        services.AddTransient(CreateAddCreditViewModel);
        services.AddTransient(CreateAccountManagementViewModel);
        services.AddTransient(CreateCreateAccountViewModel);
        services.AddTransient(CreateProfileViewModel);
        services.AddTransient(CreateChangePasswordViewModel);

        // Add DbContext with scoped lifetime
        var connectionString = Configuration.GetConnectionString("JewelryAuctionDatabase");
        services.AddDbContext<JewelryAuctionContext>(options =>
        {
            options.UseSqlServer(connectionString);
        }, ServiceLifetime.Scoped);

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
            serviceProvider.GetRequiredService<IAuctionService>(),
            serviceProvider.GetRequiredService<IBidService>(),
            CreateJewelryPageNavigationService(serviceProvider));
    }
    private PastAuctionsViewModel CreatePastAuctionsViewModel(IServiceProvider serviceProvider)
    {
        return new PastAuctionsViewModel(serviceProvider.GetRequiredService<IJewelryService>(),
            CreateJewelryPageNavigationService(serviceProvider),
            serviceProvider.GetRequiredService<IAuctionService>(),
            serviceProvider.GetRequiredService<IBidService>());
    }
    private ParameterNavigationService<JewelryListingViewModel, JewelryPageViewModel> CreateJewelryPageNavigationService(IServiceProvider serviceProvider)
    {
        return new ParameterNavigationService<JewelryListingViewModel, JewelryPageViewModel>(
            serviceProvider.GetRequiredService<NavigationStore>(), null,
            (parameter) => new JewelryPageViewModel(parameter, serviceProvider.GetRequiredService<IAccountService>(),
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
             (parameter) => new AddBidViewModel(parameter,
             serviceProvider.GetRequiredService<IAuctionService>(),
             serviceProvider.GetRequiredService<IBidService>(),
             serviceProvider.GetRequiredService<CloseModalNavigationService>(),
             CreateAddCreditNavigationService(serviceProvider),
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
            serviceProvider.GetRequiredService<CloseModalNavigationService>()
            /*serviceProvider.GetRequiredService<AccountStore>()*/);
    }
    private NavigationBarViewModel CreateNavigationBarViewModel(IServiceProvider serviceProvider)
    {
        return new NavigationBarViewModel(serviceProvider.GetRequiredService<AccountStore>(),
            CreateHomeNavigationService(serviceProvider),
            CreateLoginNavigationService(serviceProvider),
            CreateSignupNavigationService(serviceProvider),
            CreateAddJewelryNavigationService(serviceProvider),
            CreateAddAuctionNavigationService(serviceProvider),
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
    private INavigationService CreateAddAuctionNavigationService(IServiceProvider serviceProvider)
    {
        return new ModalNavigationService<AddAuctionViewModel>(
            serviceProvider.GetRequiredService<ModalNavigationStore>(),
            () => serviceProvider.GetRequiredService<AddAuctionViewModel>());
    }
    private INavigationService CreateAccountManagementNavigationService(IServiceProvider serviceProvider)
    {
        return new LayoutNavigationService<AccountManagementViewModel>(
               serviceProvider.GetRequiredService<NavigationStore>(),
               () => serviceProvider.GetRequiredService<NavigationBarViewModel>(),
               () => serviceProvider.GetRequiredService<AccountManagementViewModel>());
    }
    private LoginViewModel CreateLoginViewModel(IServiceProvider serviceProvider)
    {
        CompositeNavigationService accountManagementNavigationService = new(
            serviceProvider.GetRequiredService<CloseModalNavigationService>(),
            CreateAccountManagementNavigationService(serviceProvider));

        return new LoginViewModel(
            serviceProvider.GetRequiredService<AccountStore>(),
            accountManagementNavigationService, serviceProvider.GetRequiredService<IAccountService>(),
            serviceProvider.GetRequiredService<CloseModalNavigationService>());
    }
    private CreateAccountViewModel CreateCreateAccountViewModel(IServiceProvider serviceProvider)
    {
        return new CreateAccountViewModel(serviceProvider.GetRequiredService<IAccountService>(),
            serviceProvider.GetRequiredService<CloseModalNavigationService>());
    }

    private AccountManagementViewModel CreateAccountManagementViewModel(IServiceProvider serviceProvider)
    {
        return new AccountManagementViewModel(serviceProvider.GetRequiredService<IAccountService>(),
            CreateCreateAccountNavigationService(serviceProvider));
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
