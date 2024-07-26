using JewelryAuctionApplicationBLL.Services;
using JewelryAuctionApplicationGUI.Navigation;
using JewelryAuctionApplicationGUI.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using MessageBox1 = System.Windows.Forms.MessageBox;
using MessageBox2 = System.Windows.MessageBox;

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
            DialogResult result = MessageBox1.Show("Do you want to delete this jewelry?", "Confirmation", MessageBoxButtons.YesNoCancel);
            if (result == DialogResult.OK)
            {
                _viewModel.SelectedAccount.Account.Status = false;
                _accountService.Update(_viewModel.SelectedAccount.Account);
                _navigationService.Navigate();
                MessageBox2.Show("Account deactivated successfully", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}
