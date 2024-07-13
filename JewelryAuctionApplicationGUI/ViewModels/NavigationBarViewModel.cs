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

    public bool IsLoggedIn => _accountStore.IsLoggedIn;
    public bool IsLoggedOut => !IsLoggedIn;

    public NavigationBarViewModel(AccountStore accountStore, 
        INavigationService homeNavigationService, 
        INavigationService loginNavigationService,
        INavigationService signupNavigationService,
        INavigationService addJewelryNavigationService,
        INavigationService addAuctionNavigationService,
        INavigationService pastAuctionNavigationService,
        INavigationService addCreditNavigationService)
    {
        _accountStore = accountStore;
        NavigateHomeCommand = new NavigateCommand(homeNavigationService);
        NavigateLoginCommand = new NavigateCommand(loginNavigationService);
        NavigateSignupCommand = new NavigateCommand(signupNavigationService);
        NavigateLogoutCommand = new LogoutCommand(_accountStore, homeNavigationService);
        NavigateAddJewelryCommand = new NavigateCommand(addJewelryNavigationService);
        NavigateAddAuctionCommand = new NavigateCommand(addAuctionNavigationService);
        NavigatePastAuctionCommand = new NavigateCommand(pastAuctionNavigationService);
        NavigateAddCreditCommand = new NavigateCommand(addCreditNavigationService);

        _accountStore.CurrentAccountChanged += OnCurrentAccountChanged;
    }

    private void OnCurrentAccountChanged()
    {
        OnPropertyChanged(nameof(IsLoggedIn));
        OnPropertyChanged(nameof(IsLoggedOut));
    }

    public override void Dispose()
    {
        _accountStore.CurrentAccountChanged -= OnCurrentAccountChanged;
        base.Dispose();
    }
}
