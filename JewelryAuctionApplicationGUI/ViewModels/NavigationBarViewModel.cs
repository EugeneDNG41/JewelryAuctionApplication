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
    public ICommand NavigateAddJewelryCommand { get; }
    public ICommand NavigateAddAuctionCommand { get; }
    public ICommand NavigatePastAuctionCommand { get; }
    public ICommand NavigateAddCreditCommand { get; }
    public ICommand NavigateProfileCommand { get; }

    public bool IsLoggedIn => _accountStore.IsLoggedIn;
    public bool IsLoggedOut => !IsLoggedIn;
    public bool IsAdmin => _accountStore.IsAdmin;
    public bool IsUser => _accountStore.IsUser;
    public bool IsStaff => _accountStore.IsStaff;
    public bool IsManager => _accountStore.IsManager;

    public NavigationBarViewModel(AccountStore accountStore, 
        INavigationService homeNavigationService, 
        INavigationService loginNavigationService,
        INavigationService signupNavigationService,
        INavigationService addJewelryNavigationService,
        INavigationService addAuctionNavigationService,
        INavigationService pastAuctionNavigationService,
        INavigationService addCreditNavigationService,
        INavigationService profileNavigationService)
    {
        _accountStore = accountStore;
        NavigateHomeCommand = new NavigateCommand(homeNavigationService); //switch functionality + text
        NavigateLoginCommand = new NavigateCommand(loginNavigationService); //check
        NavigateSignupCommand = new NavigateCommand(signupNavigationService); //check
        NavigateLogoutCommand = new LogoutCommand(_accountStore, homeNavigationService); //check
        NavigateAddJewelryCommand = new NavigateCommand(addJewelryNavigationService);
        NavigateAddAuctionCommand = new NavigateCommand(addAuctionNavigationService);
        NavigatePastAuctionCommand = new NavigateCommand(pastAuctionNavigationService); //switch functionality + text
        NavigateAddCreditCommand = new NavigateCommand(addCreditNavigationService);
        NavigateProfileCommand = new NavigateCommand(profileNavigationService);

        _accountStore.CurrentAccountChanged += OnCurrentAccountChanged;
    }

    private void OnCurrentAccountChanged()
    {
        OnPropertyChanged(nameof(IsLoggedIn));
        OnPropertyChanged(nameof(IsLoggedOut));
        OnPropertyChanged(nameof(IsAdmin));
        OnPropertyChanged(nameof(IsUser));
        OnPropertyChanged(nameof(IsStaff));
        OnPropertyChanged(nameof(IsManager));
    }

    public override void Dispose()
    {
        _accountStore.CurrentAccountChanged -= OnCurrentAccountChanged;
        base.Dispose();
    }
}
