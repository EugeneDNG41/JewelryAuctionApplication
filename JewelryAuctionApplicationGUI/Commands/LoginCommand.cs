
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
        if (account != null)
        {  
            _accountStore.CurrentAccount = account;
            MessageBox.Show("Login successful!");
            if (account.Role == Role.ADMIN)
            {
                _accountManagement.Navigate();
            } else if (account.Role == Role.USER)
            {
                _closeModal.Navigate();
            }
        } else
        {
            _viewModel.ErrorMessage = "Invalid username or password!";
            return;
        }        
    }
}
