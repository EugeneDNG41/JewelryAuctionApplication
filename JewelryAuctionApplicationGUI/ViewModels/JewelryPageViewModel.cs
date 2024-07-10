using JewelryAuctionApplicationBLL.Services;
using JewelryAuctionApplicationBLL.Stores;
using JewelryAuctionApplicationDAL.Models;
using JewelryAuctionApplicationGUI.Commands;
using JewelryAuctionApplicationGUI.Navigation;
using System.Windows.Input;

namespace JewelryAuctionApplicationGUI.ViewModels;

public class JewelryPageViewModel : BaseViewModel
{
    private readonly AccountStore _accountStore;
    private readonly ParameterNavigationService<JewelryListingViewModel, AddBidViewModel> _navigationAddBidCommand;
    private readonly INavigationService _loginNavigationService;
    private readonly Func<NavigationBarViewModel> _createNavigationBarViewModel;
    public JewelryListingViewModel JewelryListing { get; }
    public NavigationBarViewModel NavigationBarViewModel { get; private set; }
    public ICommand NavigateAddBidCommand { get; private set; }
    public JewelryPageViewModel(JewelryListingViewModel jewelryListing, 
        IBidService bidService, Func<NavigationBarViewModel> navigationBarViewModel,
        ParameterNavigationService<JewelryListingViewModel, AddBidViewModel> navigateAddBidCommand,
        INavigationService loginNavigationService,
        AccountStore accountStore)
    {
        JewelryListing = jewelryListing;
        _accountStore = accountStore;
        _navigationAddBidCommand = navigateAddBidCommand;
        _loginNavigationService = loginNavigationService;
        _createNavigationBarViewModel = navigationBarViewModel;
        _accountStore.CurrentAccountChanged += OnCurrentAccountChanged;
        UpdateButtonAndNavBar();
    }
    private void OnCurrentAccountChanged()
    {
        UpdateButtonAndNavBar();
        OnPropertyChanged(nameof(NavigateAddBidCommand));
        OnPropertyChanged(nameof(NavigationBarViewModel));
    }
    private void UpdateButtonAndNavBar()
    {
        if (_accountStore.CurrentAccount != null && _accountStore.CurrentAccount.Role == Role.USER)
        {
            NavigateAddBidCommand = new NavigateAddBidCommand(JewelryListing, _navigationAddBidCommand);
        }
        else
        {
            NavigateAddBidCommand = new NavigateCommand(_loginNavigationService);
        }
        NavigationBarViewModel = _createNavigationBarViewModel();
    }
    public override void Dispose()
    {
        _accountStore.CurrentAccountChanged -= OnCurrentAccountChanged;
        base.Dispose();
    }
}
