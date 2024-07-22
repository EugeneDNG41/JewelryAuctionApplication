using JewelryAuctionApplicationBLL.Services;
using JewelryAuctionApplicationGUI.Navigation;
using JewelryAuctionApplicationGUI.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace JewelryAuctionApplicationGUI.Commands;

public class DeleteAccountCommand : BaseCommand
{
    private readonly AccountManagementViewModel _viewModel;
    private readonly IAccountService _accountService;
    private readonly INavigationService _navigationService;
    public DeleteAccountCommand(AccountManagementViewModel viewModel, 
        IAccountService accountService, 
        INavigationService returnAccountManagementNavigationService)
    {
        _viewModel = viewModel;
        _accountService = accountService;
        _navigationService = returnAccountManagementNavigationService;
    }
    public override void Execute(object parameter)
    {
        if (_viewModel.SelectedAccount != null)
        {
            _viewModel.SelectedAccount.Account.Status = false;
            _accountService.Update(_viewModel.SelectedAccount.Account);
            _navigationService.Navigate();
            MessageBox.Show("Account deactivated", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
