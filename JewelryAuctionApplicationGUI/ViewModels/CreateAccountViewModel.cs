using JewelryAuctionApplicationBLL.Services;
using JewelryAuctionApplicationDAL.Models;
using JewelryAuctionApplicationGUI.Commands;
using JewelryAuctionApplicationGUI.Navigation;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;

namespace JewelryAuctionApplicationGUI.ViewModels;

public class CreateAccountViewModel : BaseViewModel
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
            }
            else if (!Regex.IsMatch(Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.IgnoreCase))
            {
                AddError("Wrong email address format", nameof(Email));
            }
            OnErrorsChanged(nameof(Email));
        }
    }
    private int _role;
    public int Role
    {
        get => _role;
        set
        {
            _role = value;
            OnPropertyChanged(nameof(Role));
        }
    }
    public ObservableCollection<string> Roles { get; private set; }
    public ICommand CreateAccountCommand { get; }
    public ICommand CloseModalCommand { get; }

    public CreateAccountViewModel(IAccountService accountService, INavigationService closeModalNavigationServce)
    {
        CreateAccountCommand = new CreateAccountCommand(this, accountService , closeModalNavigationServce);
        CloseModalCommand = new NavigateCommand(closeModalNavigationServce);
        GenerateRoleList();
    }
    private void GenerateRoleList()
    {
        Roles = new ObservableCollection<string>();
        foreach (Role role in Enum.GetValues(typeof(Role)))
        {
            string roleString = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(role.ToString().ToLower());
            Roles.Add(roleString);
        }
    }
    private readonly Dictionary<string, List<string>> _propertyErrors = new();
    public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;
    public bool HasErrors => _propertyErrors.Any();
    public bool CanClick => !HasErrors;
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
