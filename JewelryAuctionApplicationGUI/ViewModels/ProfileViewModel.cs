using JewelryAuctionApplicationBLL.Services;
using JewelryAuctionApplicationBLL.Stores;
using JewelryAuctionApplicationDAL.Models;
using JewelryAuctionApplicationGUI.Commands;
using JewelryAuctionApplicationGUI.Navigation;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;

namespace JewelryAuctionApplicationGUI.ViewModels;
public class ProfileViewModel : BaseViewModel, INotifyDataErrorInfo
{
    private readonly AccountStore _accountStore;
    public Account? Account => _accountStore.CurrentAccount;
    public ICommand UpdateProfileCommand { get; }
    public ICommand DeleteProfileCommand { get; }
    public ICommand NavigateChangePasswordCommand {  get; }
    public ICommand NavigateWonItem {  get; }
    private readonly Dictionary<string, List<string>> _propertyErrors = new();
    public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;
    public bool HasErrors => _propertyErrors.Any();
    public bool CanClick => !HasErrors;

    public ProfileViewModel(AccountStore accountStore,
                            IAccountService accountService,
                            INavigationService changePasswordNavigationService)
    {
        _accountStore = accountStore;
        GetAccountInformation();
        UpdateProfileCommand = new UpdateProfileCommand(accountService, this);
        DeleteProfileCommand = new DeleteProfileCommand(accountService, this);
        NavigateChangePasswordCommand = new NavigateCommand(changePasswordNavigationService);
    }

    private string username;
    public string Username
    {
        get => username;
        set
        {
            username = value;
            OnPropertyChanged(nameof(Username));
            ClearErrors(nameof(Username));

            if (string.IsNullOrEmpty(Username)) //check for error
            {
                AddError("Required", nameof(Username));
            }
            OnErrorsChanged(nameof(Username));
        }
    }

    private string fullName;
    public string FullName
    {
        get => fullName;
        set
        {
            fullName = value;
            OnPropertyChanged(nameof(FullName));
            ClearErrors(nameof(FullName));

            if (string.IsNullOrEmpty(FullName)) //check for error
            {
                AddError("Required", nameof(FullName));
            }
            OnErrorsChanged(nameof(FullName));
        }
    }

    //private string password;
    //public string Password
    //{
    //    get => password;
    //    set
    //    {
    //        password = value;
    //        OnPropertyChanged(nameof(Password));
    //        ClearErrors(nameof(Password));
    //    }
    //}

    private string email;
    public string Email
    {
        get => email;
        set
        {
            email = value;
            OnPropertyChanged(nameof(Email));
            ClearErrors(nameof(Email));

            if (string.IsNullOrEmpty(Email)) //check for error
            {
                AddError("Required", nameof(Email));
            }
            OnErrorsChanged(nameof(Email));
        }
    }
    private void GetAccountInformation()
    {
        if (Account != null)
        {
            Username = Account.Username;
            FullName = Account.FullName;
            Email = Account.Email;
        }
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
