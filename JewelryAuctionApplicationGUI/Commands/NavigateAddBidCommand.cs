using JewelryAuctionApplicationGUI.Navigation;
using JewelryAuctionApplicationGUI.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JewelryAuctionApplicationGUI.Commands;

public class NavigateAddBidCommand : BaseCommand
{
    private readonly ParameterNavigationService<JewelryListingViewModel, AddBidViewModel> _navigationService;
    private readonly JewelryListingViewModel _jewelryListing;
    public NavigateAddBidCommand(JewelryListingViewModel jewelryListing, 
        ParameterNavigationService<JewelryListingViewModel, AddBidViewModel>
               navigationService)
    {
        _jewelryListing = jewelryListing;
        _navigationService = navigationService;
    }

    public override void Execute(object parameter)
    {
        _navigationService.Navigate(_jewelryListing);
    }
}
