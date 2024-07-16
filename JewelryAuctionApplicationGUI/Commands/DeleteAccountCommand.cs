using JewelryAuctionApplicationBLL.Services;
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
    public DeleteAccountCommand(AccountManagementViewModel viewModel, IAccountService accountService)
    {
        _viewModel = viewModel;
        _accountService = accountService;
    }
    public override void Execute(object parameter)
    {
        if (_viewModel.SelectedAccount != null)
        {
            _viewModel.SelectedAccount.Status = false;
            _accountService.Update(_viewModel.SelectedAccount);
            MessageBox.Show("Account deactivated!");
        }
    }
}
