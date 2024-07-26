using JewelryAuctionApplicationGUI.Commands;
using JewelryAuctionApplicationBLL.Stores;
using JewelryAuctionApplicationDAL.Models;
using System.Windows.Input;
using JewelryAuctionApplicationGUI.Navigation;

namespace JewelryAuctionApplicationGUI.ViewModels;

public class NavigationBarViewModel : BaseViewModel
{
    private readonly AccountStore _accountStore;
    private string greetings;
    public string Greetings
    {
        get => greetings;
        set
        {
            greetings = value;
            OnPropertyChanged(nameof(Greetings));
        }
    }

    public ICommand NavigateHomeCommand { get; }
    public ICommand NavigateLoginCommand { get; }
    public ICommand NavigateLogoutCommand { get; }
    public ICommand NavigateSignupCommand { get; }
    public ICommand NavigateAccountManagementCommand { get; }
    public ICommand NavigateJewelryManagementCommand { get; }
    public ICommand NavigatePastAuctionCommand { get; }
    public ICommand NavigateAddCreditCommand { get; }
    public ICommand NavigateProfileCommand { get; }

    public bool IsLoggedIn => _accountStore.IsLoggedIn;
    public bool IsLoggedOut => !IsLoggedIn;
    public bool IsAdminOrManager => _accountStore.IsAdmin || _accountStore.IsManager;
    public bool IsUser => _accountStore.IsUser;
    public bool IsUserOrGuest => IsUser || IsLoggedOut;
    public bool IsStaff => _accountStore.IsStaff || IsAdminOrManager;

    public NavigationBarViewModel(AccountStore accountStore, 
        INavigationService homeNavigationService, 
        INavigationService loginNavigationService,
        INavigationService signupNavigationService,
        INavigationService accountManagementNavigationService,
        INavigationService jewelryManagementNavigationService,
        INavigationService pastAuctionNavigationService,
        INavigationService addCreditNavigationService,
        INavigationService profileNavigationService)
    {
        _accountStore = accountStore;
        NavigateHomeCommand = new NavigateCommand(homeNavigationService); //switch functionality + text
        NavigateLoginCommand = new NavigateCommand(loginNavigationService); //check
        NavigateSignupCommand = new NavigateCommand(signupNavigationService); //check
        NavigateLogoutCommand = new LogoutCommand(_accountStore, homeNavigationService); //check
        NavigateAccountManagementCommand = new NavigateCommand(accountManagementNavigationService); //switch functionality + text
        NavigateJewelryManagementCommand = new NavigateCommand(jewelryManagementNavigationService);
        NavigatePastAuctionCommand = new NavigateCommand(pastAuctionNavigationService); //switch functionality + text
        NavigateAddCreditCommand = new NavigateCommand(addCreditNavigationService);
        NavigateProfileCommand = new NavigateCommand(profileNavigationService);

        UpdateGreetings();
        _accountStore.CurrentAccountChanged += OnCurrentAccountChanged;
    }

    private void OnCurrentAccountChanged()
    {
        OnPropertyChanged(nameof(IsLoggedIn));
        OnPropertyChanged(nameof(IsLoggedOut));
        OnPropertyChanged(nameof(IsAdminOrManager));
        OnPropertyChanged(nameof(IsUser));
        OnPropertyChanged(nameof(IsUserOrGuest));
        OnPropertyChanged(nameof(IsStaff));
        UpdateGreetings();
    }
    private void UpdateGreetings()
    {
        if (_accountStore.CurrentAccount == null)
        {
            Greetings = "Welcome, Guest";
        } else
        {
            Greetings = $"Hi, {_accountStore.CurrentAccount.Role.ToString().ToLower()} {_accountStore.CurrentAccount.Username}";
            if (_accountStore.CurrentAccount.Role == Role.USER)
            {
                Greetings += $". Your current credit balance is {_accountStore.CurrentAccount.Credit}";
            }
        }
    }

    public override void Dispose()
    {
        _accountStore.CurrentAccountChanged -= OnCurrentAccountChanged;
        base.Dispose();
    }
}
