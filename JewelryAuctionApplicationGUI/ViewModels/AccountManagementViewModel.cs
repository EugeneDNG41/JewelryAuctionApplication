using JewelryAuctionApplicationBLL.Services;
using JewelryAuctionApplicationBLL.Stores;
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
    private readonly AccountStore _accountStore;
    private readonly IAccountService _accountService;
    public ObservableCollection<AccountInformationViewModel> Accounts { get; private set; }
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
    private int _statusFilter;
    public int StatusFilter
    {
        get => _statusFilter;
        set
        {
            _statusFilter = value;
            OnPropertyChanged(nameof(StatusFilter));
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
    private AccountInformationViewModel? _selectedAccount;
    public AccountInformationViewModel? SelectedAccount
    {
        get => _selectedAccount;
        set
        {
            _selectedAccount = value ?? null;
            OnPropertyChanged(nameof(SelectedAccount));
            OnPropertyChanged(nameof(CanClick));
            UpdateUpdateButton();
        }
    }
    public ObservableCollection<string> Roles { get; private set; }
    public ObservableCollection<string> Statuses =>
        new ObservableCollection<string> { "All", "Active", "Deleted" };
    public ObservableCollection<string> SortOptions =>
        new ObservableCollection<string> { "All", "Username", "Name", "Email", "Credit" };
    public ObservableCollection<string> SortOrder =>
        new ObservableCollection<string> { "Default", "Ascending", "Descending" };
    public ICommand DeleteAccountCommand { get; }
    public ICommand NavigateCreateAccountCommand { get; }
    public ICommand ResetPasswordCommand { get; }
    public ICommand NavigateUpdateAccountCommand { get; private set; }
    private readonly ParameterNavigationService<Account, UpdateAccountViewModel> _navigateUpdateAccountService;
    public bool CanClick => SelectedAccount != null;
    public AccountManagementViewModel(AccountStore accountStore,
        IAccountService accountService,
        IBidService bidService,
        INavigationService createAccountNavigationService,
        INavigationService returnAccountManagementNavigationService,
        ParameterNavigationService<Account, UpdateAccountViewModel> navigateUpdateAccountService)
    {
        _accountStore = accountStore;
        _accountService = accountService;
        InitializeAccountList(bidService);
        GenerateRoleList();
        AccountCollectionView = CollectionViewSource.GetDefaultView(Accounts);
        AccountCollectionView.Filter = AccountFilter;
        DeleteAccountCommand = new DeleteAccountCommand(this, accountService, returnAccountManagementNavigationService);
        NavigateCreateAccountCommand = new NavigateCommand(createAccountNavigationService);
        ResetPasswordCommand = new ResetPasswordCommand(this, accountService);
        _navigateUpdateAccountService = navigateUpdateAccountService;
        UpdateUpdateButton();
    }
    private void UpdateUpdateButton()
    {
        if (SelectedAccount != null)
        {
            NavigateUpdateAccountCommand = new NavigateUpdateAccountCommand(SelectedAccount.Account, _navigateUpdateAccountService);
        }
        OnPropertyChanged(nameof(NavigateUpdateAccountCommand));
    }
    private void InitializeAccountList(IBidService bidService)
    {
        Accounts = new ObservableCollection<AccountInformationViewModel>();
        var accounts = _accountService.GetAll();
        Accounts = new ObservableCollection<AccountInformationViewModel>(accounts.Select(a => new AccountInformationViewModel(a, bidService)));
    }
    private void GenerateRoleList()
    {
        Roles = new ObservableCollection<string>
        {
            "All"
        };
        foreach (Role role in Enum.GetValues(typeof(Role)))
        {
            string roleString = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(role.ToString().ToLower());
            Roles.Add(roleString);
        }
    }
    private bool AccountFilter(object obj)
    {
        if (obj is AccountInformationViewModel accountInfo)
        {
            bool roleMatch = RoleFilter == 0 || accountInfo.Account.Role == (Role)(RoleFilter - 1);
            bool nameMatch = string.IsNullOrEmpty(NameFilter) || accountInfo.Account.FullName.Contains(NameFilter, StringComparison.InvariantCultureIgnoreCase);
            bool usernameMatch = string.IsNullOrEmpty(UsernameFilter) || accountInfo.Account.Username.Contains(UsernameFilter, StringComparison.InvariantCultureIgnoreCase);
            bool emailMatch = string.IsNullOrEmpty(EmailFilter) || accountInfo.Account.Email.Contains(EmailFilter, StringComparison.InvariantCultureIgnoreCase);
            bool statusMatch = StatusFilter == 0 || (StatusFilter == 1 && accountInfo.Account.Status) || (StatusFilter == 2 && !accountInfo.Account.Status);

            return roleMatch && nameMatch && usernameMatch && emailMatch && statusMatch;
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
                    AccountCollectionView.SortDescriptions.Add(new SortDescription("Account.Username", direction));
                    break;
                case 2:
                    AccountCollectionView.SortDescriptions.Add(new SortDescription("Account.FullName", direction));
                    break;
                case 3:
                    AccountCollectionView.SortDescriptions.Add(new SortDescription("Account.Email", direction));
                    break;
                case 4:
                    AccountCollectionView.SortDescriptions.Add(new SortDescription("Account.Credit", direction));
                    break;
                default:
                    break;
            }
        }
    }
}
