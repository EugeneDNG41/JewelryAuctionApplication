using JewelryAuctionSystem.Commands;
using JewelryAuctionSystem.Services;
using JewelryAuctionSystem.Stores;
using System.Windows.Input;

namespace JewelryAuctionSystem.ViewModels;

public class LoginViewModel : BaseViewModel
{
    private string _username;
    public string Username
    {
        get
        {
            return _username;
        }
        set
        {
            _username = value;
            OnPropertyChanged(nameof(Username));
        }
    }
    private string _password;
    public string Password
    {
        get
        {
            return _password;
        }
        set
        {
            _password = value;
            OnPropertyChanged(nameof(Password));
        }
    }
    private string _errorMessage;
    public string ErrorMessage
    {
        get
        {
            return _errorMessage;
        }

        set
        {
            _errorMessage = value;
            OnPropertyChanged(nameof(ErrorMessage));
        }
    }
    public ICommand LoginCommand { get; }
    
    public LoginViewModel(AccountStore accountStore, INavigationService loginNavigationService)
    {
        LoginCommand = new LoginCommand(this, accountStore, loginNavigationService);
    }
}
