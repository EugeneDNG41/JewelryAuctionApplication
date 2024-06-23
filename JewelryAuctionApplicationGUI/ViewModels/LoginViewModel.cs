
using JewelryAuctionApplicationBLL.Services;
using JewelryAuctionApplicationBLL.Stores;
using JewelryAuctionApplicationGUI.Commands;
using JewelryAuctionApplicationGUI.Navigation;
using System.Collections;
using System.ComponentModel;
using System.Windows.Input;

namespace JewelryAuctionApplicationGUI.ViewModels;

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

            ClearErrors(nameof(Username)); //clear previous error

            if (string.IsNullOrEmpty(Username)) //check for error
            {
                AddError("Username is empty", nameof(Username));
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

            ClearErrors(nameof(Password));
            if (string.IsNullOrEmpty(Password))
            {
                AddError("Password is empty", nameof(Password));
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
    public ICommand CloseModalCommand { get; }

    private readonly Dictionary<string, List<string>> _propertyErrors = new();
    public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;
    public bool HasErrors => _propertyErrors.Any();
    public bool CanClick => !HasErrors;

    public LoginViewModel(AccountStore accountStore, 
        INavigationService loginNavigationService, 
        IAccountService accountService,
        INavigationService closeModalNavigationService)
    {
        LoginCommand = new LoginCommand(this, accountStore, loginNavigationService, accountService);       
        CloseModalCommand = new CloseModalCommand(closeModalNavigationService);
        
        /*_username = string.Empty;
        _password = string.Empty;
        _errorMessage = string.Empty;*/
    }

    public IEnumerable GetErrors(string propertyName)
    {
        return _propertyErrors.GetValueOrDefault(propertyName, null);
    }
    public void AddError(string errorMessage, string propertyName)
    {
        if (!_propertyErrors.ContainsKey(propertyName))
        {
            _propertyErrors.Add(propertyName, new List<string>());
        }
        _propertyErrors[propertyName].Add(errorMessage);
        OnErrorsChanged(propertyName);
    }

    private void OnErrorsChanged(string propertyName)
    {
        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        OnPropertyChanged(nameof(CanClick));
    }

    public void ClearErrors(string propertyName)
    {
        if (_propertyErrors.Remove(propertyName))
        {
            OnErrorsChanged(propertyName); //make sure that the change should be notified accordingly
        }
    }
}
