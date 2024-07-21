using JewelryAuctionApplicationBLL.Services;
using JewelryAuctionApplicationDAL.Models;
using JewelryAuctionApplicationGUI.Commands;
using JewelryAuctionApplicationGUI.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;

namespace JewelryAuctionApplicationGUI.ViewModels;

public class AccountManagementViewModel : BaseViewModel
{
    private readonly IAccountService _accountService;
    public ObservableCollection<Account> Accounts { get; private set; }
    public ICollectionView AccountCollectionView { get; private set; }
    private string _usernameFilter = string.Empty;
    public string UsernameFilter
    {
        get => _usernameFilter;
        set
        {
            _usernameFilter = value;
            OnPropertyChanged(nameof(UsernameFilter));
            AccountCollectionView.Refresh();
        }
    }
    private string _nameFilter = string.Empty;
    public string NameFilter
    {
        get => _nameFilter;
        set
        {
            _nameFilter = value;
            OnPropertyChanged(nameof(NameFilter));
            AccountCollectionView.Refresh();
        }
    }
    private string _emailFilter = string.Empty;
    public string EmailFilter
    {
        get => _emailFilter;
        set
        {
            _emailFilter = value;
            OnPropertyChanged(nameof(EmailFilter));
            AccountCollectionView.Refresh();
        }
    }
    private int _roleFilter;
    public int RoleFilter
    {
        get => _roleFilter;
        set
        {
            _roleFilter = value;
            OnPropertyChanged(nameof(RoleFilter));
            AccountCollectionView.Refresh();
        }
    }
    private int _selectedSortOption;
    public int SelectedSortOption
    {
        get => _selectedSortOption;
        set
        {
            _selectedSortOption = value;
            OnPropertyChanged(nameof(SelectedSortOption));
            UpdateSorting();
        }
    }

    private int _selectedSortOrder;
    public int SelectedSortOrder
    {
        get => _selectedSortOrder;
        set
        {
            _selectedSortOrder = value;
            OnPropertyChanged(nameof(SelectedSortOrder));
            UpdateSorting();
        }
    }
    private Account? _selectedAccount;
    public Account? SelectedAccount
    {
        get => _selectedAccount;
        set
        {
            _selectedAccount = value ?? null;
            OnPropertyChanged(nameof(SelectedAccount));
            OnPropertyChanged(nameof(CanClick));
        }
    }
    public ObservableCollection<string> Roles { get; private set; }
    public ObservableCollection<string> SortOptions =>
        new ObservableCollection<string> { "All", "Username", "Name", "Email", "Credit" };
    public ObservableCollection<string> SortOrder =>
        new ObservableCollection<string> { "Default", "Ascending", "Descending" };
    public ICommand DeleteAccountCommand { get; }
    public ICommand NavigateCreateAccountCommand { get; }
    public ICommand ResetPasswordCommand { get; }
    public bool CanClick => SelectedAccount != null;
    public AccountManagementViewModel(IAccountService accountService, INavigationService createAccountNavigationService)
    {
        _accountService = accountService;
        InitializeAccountList();
        GenerateRoleList();
        AccountCollectionView = CollectionViewSource.GetDefaultView(Accounts);
        AccountCollectionView.Filter = AccountFilter;
        DeleteAccountCommand = new DeleteAccountCommand(this, accountService);
        NavigateCreateAccountCommand = new NavigateCommand(createAccountNavigationService);
        ResetPasswordCommand = new ResetPasswordCommand(this, accountService);
    }
    private void InitializeAccountList()
    {
        Accounts = new ObservableCollection<Account>(_accountService.GetAll());
    }
    private void GenerateRoleList()
    {
        Roles = new ObservableCollection<string>
        {
            "All Roles"
        };
        foreach (Role role in Enum.GetValues(typeof(Role)))
        {
            string roleString = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(role.ToString().ToLower());
            Roles.Add(roleString);
        }
    }
    private bool AccountFilter(object obj)
    {
        if (obj is Account account)
        {
            bool roleMatch = RoleFilter == 0 || account.Role == (Role)(RoleFilter - 1);
            bool nameMatch = string.IsNullOrEmpty(NameFilter) || account.FullName.Contains(NameFilter, StringComparison.InvariantCultureIgnoreCase);
            bool usernameMatch = string.IsNullOrEmpty(UsernameFilter) || account.Username.Contains(UsernameFilter, StringComparison.InvariantCultureIgnoreCase);
            bool emailMatch = string.IsNullOrEmpty(EmailFilter) || account.Email.Contains(EmailFilter, StringComparison.InvariantCultureIgnoreCase);

            return roleMatch && nameMatch && usernameMatch && emailMatch;
        }
        return false;
    }

    private void UpdateSorting()
    {
        AccountCollectionView.SortDescriptions.Clear();
        AccountCollectionView.Refresh();
        if (SelectedSortOption != 0 && SelectedSortOrder != 0)
        {
            var direction = SelectedSortOrder == 1 ? ListSortDirection.Ascending : ListSortDirection.Descending;
            switch (SelectedSortOption)
            {
                case 1:
                    AccountCollectionView.SortDescriptions.Add(new SortDescription("Username", direction));
                    break;
                case 2:
                    AccountCollectionView.SortDescriptions.Add(new SortDescription("FullName", direction));
                    break;
                case 3:
                    AccountCollectionView.SortDescriptions.Add(new SortDescription("Email", direction));
                    break;
                case 4:
                    AccountCollectionView.SortDescriptions.Add(new SortDescription("Credit", direction));
                    break;
                default:
                    break;
            }
        }
    }
}
