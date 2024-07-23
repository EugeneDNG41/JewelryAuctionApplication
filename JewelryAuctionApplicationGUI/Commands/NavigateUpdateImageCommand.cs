using JewelryAuctionApplicationDAL.Models;
using JewelryAuctionApplicationGUI.Navigation;
using JewelryAuctionApplicationGUI.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JewelryAuctionApplicationGUI.Commands;

public class NavigateUpdateImageCommand : BaseCommand
{
    private readonly Jewelry _jewelry;
    private readonly ParameterNavigationService<Jewelry, UpdateImageViewModel> _navigationService;
    public NavigateUpdateImageCommand(Jewelry jewelry, ParameterNavigationService<Jewelry, UpdateImageViewModel> navigationService)
    {
        _jewelry = jewelry;
        _navigationService = navigationService;
    }

    public override void Execute(object parameter)
    {
        _navigationService.Navigate(_jewelry);
    }
}
