using JewelryAuctionApplicationBLL.Stores;
using JewelryAuctionApplicationGUI.Navigation;

namespace JewelryAuctionApplicationGUI.Commands;

public class LogoutCommand : BaseCommand
{
    private readonly AccountStore _accountStore;
    private readonly INavigationService _navigationService;

    public LogoutCommand(AccountStore accountStore,
        INavigationService navigationService)
    {
        _accountStore = accountStore;
        _navigationService = navigationService;
    }
    public override void Execute(object parameter)
    {
        _accountStore.Logout();
        _navigationService.Navigate();
    }
}
