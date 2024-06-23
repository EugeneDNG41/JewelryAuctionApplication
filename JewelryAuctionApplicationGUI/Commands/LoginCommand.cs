
using JewelryAuctionApplicationDAL.Models;
using JewelryAuctionApplicationBLL.Services;
using JewelryAuctionApplicationBLL.Stores;
using JewelryAuctionApplicationGUI.ViewModels;
using JewelryAuctionApplicationGUI.Navigation;


namespace JewelryAuctionApplicationGUI.Commands;

public class LoginCommand : BaseCommand
{
    private readonly LoginViewModel _viewModel;
    private readonly AccountStore _accountStore;
    private readonly INavigationService _navigationService;
    private readonly IAccountService _accountService;

    public LoginCommand(LoginViewModel viewModel, 
        AccountStore accountStore, 
        INavigationService navigationService, 
        IAccountService accountService)
    {
        _viewModel = viewModel;
        _accountStore = accountStore;
        _navigationService = navigationService;
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
            _navigationService.Navigate();
        } else
        {
            _viewModel.ErrorMessage = "Invalid username or password!";
            return;
        }        
    }
}
