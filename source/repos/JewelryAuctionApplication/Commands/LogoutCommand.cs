using JewelryAuctionSystem.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JewelryAuctionSystem.Commands;

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
