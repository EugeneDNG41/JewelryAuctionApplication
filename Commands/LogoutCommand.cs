using JewelryAuctionApplication.Commands;
using JewelryAuctionApplication.Stores;

namespace JewelryAuctionApplication.Commands;

public class LogoutCommand : BaseCommand
{
    private readonly AccountStore _accountStore;

    public LogoutCommand(AccountStore accountStore)
    {
        _accountStore = accountStore;
    }

    public override void Execute(object parameter)
    {
        _accountStore.Logout();
    }
}
