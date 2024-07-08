using JewelryAuctionApplicationBLL.Services;
using JewelryAuctionApplicationBLL.Stores;
using JewelryAuctionApplicationDAL.Models;
using JewelryAuctionApplicationGUI.Commands;
using JewelryAuctionApplicationGUI.Navigation;
using System.Windows.Input;

namespace JewelryAuctionApplicationGUI.ViewModels;

public class JewelryPageViewModel : BaseViewModel
{
    public JewelryListingViewModel JewelryListing { get; }
    public NavigationBarViewModel NavigationBarViewModel { get; }
    public ICommand NavigateAddBidCommand { get; }
    public JewelryPageViewModel(JewelryListingViewModel jewelryListing, 
        IBidService bidService, NavigationBarViewModel navigationBarViewModel,
        ParameterNavigationService<JewelryListingViewModel, AddBidViewModel> navigateAddBidCommand,
        INavigationService loginNavigationService,
        AccountStore accountStore)
    {
        JewelryListing = jewelryListing;
        NavigationBarViewModel = navigationBarViewModel;
        if (accountStore.CurrentAccount != null && accountStore.CurrentAccount.Role == Role.USER)
        {
            NavigateAddBidCommand = new NavigateAddBidCommand(jewelryListing, navigateAddBidCommand);
        } else
        {
            NavigateAddBidCommand = new NavigateCommand(loginNavigationService);
        }

    }
}
