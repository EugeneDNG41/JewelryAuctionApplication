
using JewelryAuctionApplicationDAL.Models;
using JewelryAuctionApplicationBLL.Services;
using JewelryAuctionApplicationBLL.Stores;
using JewelryAuctionApplicationGUI.ViewModels;
using JewelryAuctionApplicationGUI.Navigation;
using System.Windows;


namespace JewelryAuctionApplicationGUI.Commands;

public class LoginCommand : BaseCommand
{
    private readonly LoginViewModel _viewModel;
    private readonly AccountStore _accountStore;
    private readonly INavigationService _closeModal;
    private readonly INavigationService _accountManagement;
    private readonly IAccountService _accountService;

    public LoginCommand(LoginViewModel viewModel, 
        AccountStore accountStore, INavigationService accountManagement, 
        INavigationService closeModal, 
        IAccountService accountService)
    {
        _viewModel = viewModel;
        _accountStore = accountStore;
        _accountManagement = accountManagement;
        _closeModal = closeModal;
        _accountService = accountService;
    }

    public override void Execute(object parameter)
    {
        if (string.IsNullOrEmpty(_viewModel.Username))
        {
            _viewModel.AddError("Required", nameof(_viewModel.Username));
            return;
        }
        if (string.IsNullOrEmpty(_viewModel.Password))
        {
            _viewModel.AddError("Required", nameof(_viewModel.Password));
            return;
        }
        Account? account = _accountService.Authenticate(_viewModel.Username, _viewModel.Password);
        if (account != null && account.Status)
        {  
            _accountStore.CurrentAccount = account;
            MessageBox.Show("Login successful!");
            if (_accountStore.IsAdmin || _accountStore.IsManager)
            {
                _accountManagement.Navigate();
            } else if (_accountStore.IsUser)
            {
                _closeModal.Navigate();
            }
        } else if (account != null && !account.Status)
        {
            _viewModel.ErrorMessage = "Account is deactivated!";
            return;
        } else
        {
            _viewModel.ErrorMessage = "Invalid username or password!";
            return;
        }        
    }
}
