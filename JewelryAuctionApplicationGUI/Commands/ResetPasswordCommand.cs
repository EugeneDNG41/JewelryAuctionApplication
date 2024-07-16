using JewelryAuctionApplicationBLL.Services;
using JewelryAuctionApplicationGUI.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace JewelryAuctionApplicationGUI.Commands;

public class ResetPasswordCommand : BaseCommand
{
    private readonly AccountManagementViewModel _viewModel;
    private readonly IAccountService _accountService;
    public ResetPasswordCommand(AccountManagementViewModel viewModel, IAccountService accountService)
    {
        _viewModel = viewModel;
        _accountService = accountService;
    }

    public override void Execute(object parameter)
    {
        if (_viewModel.SelectedAccount != null)
        {
            _viewModel.SelectedAccount.Password = "123";
            _accountService.Update(_viewModel.SelectedAccount);
            MessageBox.Show("Password reset successful! (123)");
        }
    }
}
