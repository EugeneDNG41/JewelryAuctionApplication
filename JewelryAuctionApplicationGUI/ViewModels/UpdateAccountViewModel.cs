using Azure.Identity;
using JewelryAuctionApplicationBLL.Services;
using JewelryAuctionApplicationBLL.Stores;
using JewelryAuctionApplicationDAL.Models;
using JewelryAuctionApplicationGUI.Commands;
using JewelryAuctionApplicationGUI.Navigation;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;

namespace JewelryAuctionApplicationGUI.ViewModels;

public class UpdateAccountViewModel : BaseViewModel, INotifyDataErrorInfo
{
    public Account Account { get; }
    private string username;
    public string Username
    {
        get => username;
        set
        {
            username = value;
            OnPropertyChanged(nameof(Username));
            OnPropertyChanged(nameof(CanClick));
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
        get => fullName;
        set
        {
            fullName = value; 
            OnPropertyChanged(nameof(FullName));
            OnPropertyChanged(nameof(CanClick));
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
        get => email;
        set
        {
            email = value;
            OnPropertyChanged(nameof(Email));
            OnPropertyChanged(nameof(CanClick));
            ClearErrors(nameof(Email)); //clear previous error

            if (string.IsNullOrEmpty(Email)) //check for error
            {
                AddError("Required", nameof(Email));
            }
            else if (!Regex.IsMatch(Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.IgnoreCase))
            {
                AddError("Wrong email address format", nameof(Email));
            }
            OnErrorsChanged(nameof(Email));
        }
    }
    private decimal credit;
    public decimal Credit
    {
        get => credit;
        set
        {
            credit = value;
            OnPropertyChanged(nameof(Credit));
            ClearErrors(nameof(Credit)); //clear previous error
            if (Credit < BidBalance) //check for error
            {
                AddError($"Credit can't be lower than bid balance ({BidBalance})", nameof(Credit));
            }
            OnErrorsChanged(nameof(Credit));
        }
    }
    private int role;
    public int Role
    {
        get => role;
        set
        {
            role = value;
            OnPropertyChanged(nameof(Role));
        }
    }
    private int status;
    public int Status
    {
        get => status;
        set
        {
            status = value;
            OnPropertyChanged(nameof(Status));
        }
    }
    public decimal BidBalance
    {
        get
        {
            decimal bidBalance = 0;
            if (Account != null)
            {
                bidBalance = _bidService.GetBidBalanceByAccountId(Account.AccountId);
            }
            return bidBalance;
        }
    }
    private readonly IBidService _bidService;
    public ObservableCollection<string> Statuses =>
        new ObservableCollection<string> {"Active", "Deleted" };
    public ObservableCollection<string> Roles { get; private set; }
    public ICommand UpdateAccountCommand { get; }
    public ICommand CloseModalCommand { get; }
    private readonly Dictionary<string, List<string>> _propertyErrors = new();
    public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;
    public bool HasErrors => _propertyErrors.Any();
    public bool CanClick => !HasErrors;

    public UpdateAccountViewModel(Account account, 
        IAccountService accountService, IBidService bidService,
        INavigationService closeModalNavigationService,
        INavigationService returnAccountManagementNavigationService)
    {
        Account = account;
        _bidService = bidService;
        GenerateRoleList();
        username = account.Username;
        fullName = account.FullName;
        email = account.Email;
        credit = account.Credit;
        role = (int)account.Role;
        status = account.Status ? 0 : 1;
        UpdateAccountCommand = new UpdateAccountCommand(this, accountService, returnAccountManagementNavigationService);
        CloseModalCommand = new CloseModalCommand(closeModalNavigationService);
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
