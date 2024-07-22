using JewelryAuctionApplicationGUI.Commands;
using JewelryAuctionApplicationBLL.Stores;
using System.Windows.Input;
using JewelryAuctionApplicationGUI.Navigation;

namespace JewelryAuctionApplicationGUI.ViewModels;

public class NavigationBarViewModel : BaseViewModel
{
    private readonly AccountStore _accountStore;
    private string _searchText = string.Empty;
    public string SearchText
    {
        get => _searchText;
        set
        {
            _searchText = value;
            OnPropertyChanged(nameof(SearchText));
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
        INavigationService addCreditNavigationService)
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
    }

    public override void Dispose()
    {
        _accountStore.CurrentAccountChanged -= OnCurrentAccountChanged;
        base.Dispose();
    }
}
