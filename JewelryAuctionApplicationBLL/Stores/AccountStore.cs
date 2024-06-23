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

    public event Action? CurrentAccountChanged;

    public void Logout()
    {
        CurrentAccount = null;
    }
}
