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
            CurrentAccountChanged?.Invoke();
        }
    }

    public bool IsLoggedIn => CurrentAccount != null;
    public bool IsUser => CurrentAccount?.Role == Role.USER;
    public bool IsAdmin => CurrentAccount?.Role == Role.ADMIN;
    public bool IsStaff => CurrentAccount?.Role == Role.STAFF;
    public bool IsManger => CurrentAccount?.Role == Role.MANAGER;

    public event Action? CurrentAccountChanged;

    public void Logout()
    {
        CurrentAccount = null;
    }
}
