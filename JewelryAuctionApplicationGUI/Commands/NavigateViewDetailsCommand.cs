using JewelryAuctionApplicationGUI.Navigation;
using JewelryAuctionApplicationGUI.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JewelryAuctionApplicationGUI.Commands;

public class NavigateViewDetailsCommand : BaseCommand
{
    private readonly ParameterNavigationService<JewelryManagerViewModel, ViewDetailsViewModel> _navigationService;
    private readonly JewelryManagerViewModel _jewelryManager;
    public NavigateViewDetailsCommand(ParameterNavigationService<JewelryManagerViewModel, ViewDetailsViewModel> navigationService, JewelryManagerViewModel jewelryManager)
    {
        _navigationService = navigationService;
        _jewelryManager = jewelryManager;
    }

    public override void Execute(object parameter)
    {
        _navigationService.Navigate(_jewelryManager);
    }
}
