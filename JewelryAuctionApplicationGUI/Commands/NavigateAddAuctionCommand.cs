using JewelryAuctionApplicationDAL.Models;
using JewelryAuctionApplicationGUI.Navigation;
using JewelryAuctionApplicationGUI.ViewModels;


namespace JewelryAuctionApplicationGUI.Commands;

public class NavigateAddAuctionCommand : BaseCommand
{
    private readonly Jewelry _jewelry;
    private readonly ParameterNavigationService<Jewelry, AddAuctionViewModel> _navigationService;
    public NavigateAddAuctionCommand(Jewelry jewelry, ParameterNavigationService<Jewelry, AddAuctionViewModel> navigationService)
    {
        _jewelry = jewelry;
        _navigationService = navigationService;
    }

    public override void Execute(object parameter)
    {
        _navigationService.Navigate(_jewelry);
    }
}
