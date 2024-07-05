using JewelryAuctionApplicationBLL.Services;
using JewelryAuctionApplicationGUI.Navigation;
using JewelryAuctionApplicationGUI.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JewelryAuctionApplicationGUI.Commands;

public class NavigateJewelryPageCommand : BaseCommand
{
    private readonly ParameterNavigationService<JewelryListingViewModel, JewelryPageViewModel> _navigationService;
    private readonly JewelryListingViewModel _jewelryListing;
    public NavigateJewelryPageCommand(JewelryListingViewModel jewelryListing, 
        ParameterNavigationService<JewelryListingViewModel, JewelryPageViewModel> navigationService)
    {
        _jewelryListing = jewelryListing;
        _navigationService = navigationService;
    }
    public override void Execute(object parameter)
    {
        _navigationService.Navigate(_jewelryListing);
    }
}
