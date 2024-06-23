using JewelryAuctionApplicationBLL.Stores;
using JewelryAuctionApplicationGUI.Navigation;
using System.Windows.Input;

namespace JewelryAuctionApplicationGUI.Commands;

public class CloseModalCommand : BaseCommand
{
    private readonly INavigationService _closeModalService;

    public CloseModalCommand(INavigationService navigationService)
    {
        _closeModalService = navigationService;
    }
    public override void Execute(object parameter)
    {
        _closeModalService.Navigate();
    }
}