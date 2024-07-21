using JewelryAuctionApplicationDAL.Models;

namespace JewelryAuctionApplicationBLL.Stores;

public class AccountStore
{
    private Account? _currentAccount;
    public Account? CurrentAccount
    {
        get => _currentAccount;
        set
        {
            _currentAccount = value;
            UpdateAccountStatus();
            CurrentAccountChanged?.Invoke();
        }
    }

    public bool IsLoggedIn { get; private set; }
    public bool IsUser { get; private set; }
    public bool IsAdmin { get; private set; }
    public bool IsStaff { get; private set; }
    public bool IsManager { get; private set; }

    public event Action? CurrentAccountChanged;

    public void Logout()
    {
        CurrentAccount = null;
    }
    private void UpdateAccountStatus()
    {
        IsLoggedIn = _currentAccount != null;
        IsUser = _currentAccount?.Role == Role.USER;
        IsAdmin = _currentAccount?.Role == Role.ADMIN;
        IsStaff = _currentAccount?.Role == Role.STAFF;
        IsManager = _currentAccount?.Role == Role.MANAGER;
    }
}
