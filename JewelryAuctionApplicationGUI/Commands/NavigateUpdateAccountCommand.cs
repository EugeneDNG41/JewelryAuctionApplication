using JewelryAuctionApplicationDAL.Models;
using JewelryAuctionApplicationGUI.Navigation;
using JewelryAuctionApplicationGUI.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;

namespace JewelryAuctionApplicationGUI.Commands;

public class NavigateUpdateAccountCommand : BaseCommand
{
    private readonly Account _account;
    private readonly ParameterNavigationService<Account, UpdateAccountViewModel> _navigationService;
    public NavigateUpdateAccountCommand(Account account, 
        ParameterNavigationService<Account, UpdateAccountViewModel> navigationService)
    {
        _account = account;
        _navigationService = navigationService;
    }
    public override void Execute(object parameter)
    {
        _navigationService.Navigate(_account);
    }
}
