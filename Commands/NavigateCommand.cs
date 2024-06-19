using JewelryAuctionApplication.Services;
using JewelryAuctionApplication.Commands;

namespace JewelryAuctionApplication.Commands;

public class NavigateCommand : BaseCommand
{
    private readonly INavigationService _navigationService;

    public NavigateCommand(INavigationService navigationService)
    {
        _navigationService = navigationService;
    }

    public override void Execute(object parameter)
    {
        _navigationService.Navigate();
    }
}
