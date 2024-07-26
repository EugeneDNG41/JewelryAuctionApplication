using JewelryAuctionApplicationBLL.Services;
using JewelryAuctionApplicationBLL.Stores;
using JewelryAuctionApplicationDAL.Models;
using JewelryAuctionApplicationGUI.Navigation;
using JewelryAuctionApplicationGUI.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace JewelryAuctionApplicationGUI.Commands;

public class ChangePasswordCommand : BaseCommand
{
    private readonly ChangePasswordViewModel _viewModel;
    private readonly AccountStore _accountStore;
    private readonly INavigationService _closeModal;
    private readonly IAccountService _accountService;

    public ChangePasswordCommand(ChangePasswordViewModel viewModel,
        AccountStore accountStore, INavigationService closeModal,
        IAccountService accountService)
    {
        _viewModel = viewModel;
        _accountStore = accountStore;
        _closeModal = closeModal;
        _accountService = accountService;
    }

    public override void Execute(object parameter)
    {
        if (string.IsNullOrEmpty(_viewModel.OldPassword))
        {
            _viewModel.ErrorMessage = "Required";
            return;
        }
        if (string.IsNullOrEmpty(_viewModel.NewPassword))
        {
            _viewModel.ErrorMessage = "Required";
            return;
        }
        if (string.IsNullOrEmpty(_viewModel.ConfirmPassword))
        {
            _viewModel.ErrorMessage = "Required";
            return;
        }
        if (_viewModel.ConfirmPassword != _viewModel.NewPassword)
        {
            _viewModel.ErrorMessage = "New password and Confirm password are not matched";
            return;
        }
        Account? oldAccount = _accountService.Authenticate(_accountStore.CurrentAccount.Username, _viewModel.OldPassword);
        if (oldAccount != null)
        {
            _accountService.ChangePassword(oldAccount, _viewModel.NewPassword);
            Account? account = _accountService.Authenticate(oldAccount.Username, _viewModel.NewPassword);
            if (account != null)
            {
                _accountStore.CurrentAccount = account;
                MessageBox.Show("Change password successfully!", "Change Password");
                _closeModal.Navigate();
            }
            else
            {
                MessageBox.Show("Failed to change password!", "Change Password", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            
        }
        else
        {
            MessageBox.Show("Old password is wrong!", "Change Password", MessageBoxButton.OK, MessageBoxImage.Warning);
        }        

    }
}
