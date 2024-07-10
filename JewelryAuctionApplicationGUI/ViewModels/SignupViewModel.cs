using JewelryAuctionApplicationBLL.Services;
using JewelryAuctionApplicationBLL.Stores;
using JewelryAuctionApplicationGUI.Commands;
using JewelryAuctionApplicationGUI.Navigation;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.FileSystemGlobbing.Internal;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;

namespace JewelryAuctionApplicationGUI.ViewModels;

public class SignupViewModel : BaseViewModel, INotifyDataErrorInfo
{
    private string username;
    public string Username
    {
        get
        {
            return username;
        }

        set
        {
            username = value; //new value is inputted
            OnPropertyChanged(nameof(Username));

            ClearErrors(nameof(Username)); //clear previous error

            if (string.IsNullOrEmpty(Username)) //check for error
            {
                AddError("Required", nameof(Username));
            } /*else if (!Regex.IsMatch(Username, @"^[a-zA-Z0-9_]{6,20}$") || Username.Contains("admin", StringComparison.OrdinalIgnoreCase))
            {
                AddError("Invalid username", nameof(Username));
            }*/
            OnErrorsChanged(nameof(Username));
        }
    }
    private string password;
    public string Password
    {
        get
        {
            return password;
        }

        set
        {
            password = value;
            OnPropertyChanged(nameof(Password));
            ClearErrors(nameof(Password)); //clear previous error

            if (string.IsNullOrEmpty(Password)) //check for error
            {
                AddError("Required", nameof(Password));
            } /*else if (!Regex.IsMatch(Password, @"^(?=.*[A-Z])(?=.*\d)[^\s]{6,20}$"))
            {
                AddError("Invalid Password", nameof(Password));
            }*/
            
            OnErrorsChanged(nameof(Password));
        }
    }
    private string fullName;
    public string FullName
    {
        get
        {
            return fullName;
        }

        set
        {
            fullName = value;
            OnPropertyChanged(nameof(FullName));
            ClearErrors(nameof(FullName)); //clear previous error

            if (string.IsNullOrEmpty(FullName)) //check for error
            {
                AddError("Required", nameof(FullName));
            }
            /*else if (!Regex.IsMatch(FullName, @"^[a-zA-Z\s]{6,50}$"))
            {
                AddError("Invalid name format", nameof(FullName));
            }*/
            OnErrorsChanged(nameof(FullName));
        }
    }
    private string email;
    
    public string Email
    {
        get
        {
            return email;
        }

        set
        {
            email = value;
            OnPropertyChanged(nameof(Email));
            ClearErrors(nameof(Email)); //clear previous error

            if (string.IsNullOrEmpty(Email)) //check for error
            {
                AddError("Email cannot be empty", nameof(Email));
            } else if (!Regex.IsMatch(Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.IgnoreCase))
            {
                AddError("Wrong email address format", nameof(Email));
            }
            OnErrorsChanged(nameof(Email));
        }
    }
    public ICommand SignupCommand { get; }
    public ICommand CloseModalCommand { get; }
    private readonly Dictionary<string, List<string>> _propertyErrors = new();
    public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;
    public bool HasErrors => _propertyErrors.Any();
    public bool CanClick => !HasErrors;
    public SignupViewModel(
        AccountStore accountStore,        
        INavigationService signupSuccessNavigationService,
        IAccountService accountService,
        INavigationService closeModalNavigationService)
    {
        SignupCommand = new SignupCommand(this, accountStore, signupSuccessNavigationService, accountService);
        CloseModalCommand = new CloseModalCommand(closeModalNavigationService);
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
