using JewelryAuctionApplicationBLL.Services;
using JewelryAuctionApplicationDAL.Models;
using JewelryAuctionApplicationGUI.Navigation;
using JewelryAuctionApplicationGUI.ViewModels;
using System.Windows;

namespace JewelryAuctionApplicationGUI.Commands;

public class UpdateAccountCommand : BaseCommand
{
    private readonly IAccountService _accountService;
    private readonly UpdateAccountViewModel _viewModel;
    private readonly INavigationService _navigationService;
    public UpdateAccountCommand(UpdateAccountViewModel updateAccountViewModel, 
        IAccountService accountService, 
        INavigationService returnAccountManagementNavigationService)
    {
        _accountService = accountService;
        _viewModel = updateAccountViewModel;
        _navigationService = returnAccountManagementNavigationService;
    }
    public override void Execute(object parameter)
    {
        if (_accountService.GetByUsername(_viewModel.Username) != null && _accountService.GetByUsername(_viewModel.Username) != _viewModel.Account)
        {
            MessageBox.Show("Username already exists", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }
        if (_accountService.GetByEmail(_viewModel.Email) != null && _accountService.GetByEmail(_viewModel.Email) != _viewModel.Account)
        {
            MessageBox.Show("Email already exists", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }
        _viewModel.Account.Username = _viewModel.Username;
        _viewModel.Account.FullName = _viewModel.FullName;
        _viewModel.Account.Email = _viewModel.Email;
        _viewModel.Account.Credit = _viewModel.Credit;
        _viewModel.Account.Role = (Role)_viewModel.Role;
        _viewModel.Account.Status = _viewModel.Status == 0;
        _accountService.Update(_viewModel.Account);
        MessageBox.Show("Account updated successfully", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        _navigationService.Navigate();
    }
}
