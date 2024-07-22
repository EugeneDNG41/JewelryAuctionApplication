using JewelryAuctionApplicationDAL.Models;
using JewelryAuctionApplicationGUI.Navigation;
using JewelryAuctionApplicationGUI.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JewelryAuctionApplicationGUI.Commands;

public class NavigateUpdateJewelryCommand : BaseCommand
{
    private readonly JewelryListingViewModel _jewelryListing;
    private readonly ParameterNavigationService<JewelryListingViewModel, UpdateJewelryViewModel> _navigationService;
    public NavigateUpdateJewelryCommand(JewelryListingViewModel jewelrylisting, 
        ParameterNavigationService<JewelryListingViewModel, UpdateJewelryViewModel> navigationService)
    {
        _jewelryListing = jewelrylisting;
        _navigationService = navigationService;
    }
    public override void Execute(object parameter)
    {
        _navigationService.Navigate(_jewelryListing);
    }
}
