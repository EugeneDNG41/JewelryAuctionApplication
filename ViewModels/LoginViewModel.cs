using JewelryAuctionApplication.Commands;
using JewelryAuctionApplication.Services;
using JewelryAuctionApplication.Stores;
using System.Collections;
using System.ComponentModel;
using System.Windows.Input;

namespace JewelryAuctionApplication.ViewModels;

public class LoginViewModel : BaseViewModel, INotifyDataErrorInfo
{
    private string _username = string.Empty;
    public string Username
    {
        get
        {
            return _username;
        }
        set
        {
            _username = value; //new value is inputted
            OnPropertyChanged(nameof(Username));

            _errorViewModel.ClearErrors(nameof(Username)); //clear previous error

            if (string.IsNullOrEmpty(Username)) //check for error
            {
                _errorViewModel.AddError("Username is empty", nameof(Username));
            }
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

            _errorViewModel.ClearErrors(nameof(Password));
            if (string.IsNullOrEmpty(Password))
            {
                _errorViewModel.AddError("Password is empty", nameof(Password));
            }
            
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

    private readonly ErrorsViewModel _errorViewModel;

    public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

    public bool HasErrors => _errorViewModel.HasErrors;
    public bool CanClick => !HasErrors;

    public LoginViewModel(AccountStore accountStore, INavigationService loginNavigationService, IAccountService accountService)
    {
        LoginCommand = new LoginCommand(this, accountStore, loginNavigationService, accountService);
        _errorViewModel = new ErrorsViewModel();
        _errorViewModel.ErrorsChanged += ErrorViewModel_ErrorsChanged;
        _username = string.Empty;
        _password = string.Empty;
        _errorMessage = string.Empty;
    }

    private void ErrorViewModel_ErrorsChanged(object? sender, DataErrorsChangedEventArgs e)
    {
        ErrorsChanged?.Invoke(this, e);
        OnPropertyChanged(nameof(CanClick));
    }

    public IEnumerable GetErrors(string propertyName)
    {
        return _errorViewModel.GetErrors(propertyName);
    }
}
