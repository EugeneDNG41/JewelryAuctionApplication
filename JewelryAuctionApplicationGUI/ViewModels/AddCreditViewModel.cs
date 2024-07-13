using JewelryAuctionApplicationBLL.Services;
using JewelryAuctionApplicationBLL.Stores;
using JewelryAuctionApplicationDAL.Models;
using JewelryAuctionApplicationGUI.Commands;
using JewelryAuctionApplicationGUI.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace JewelryAuctionApplicationGUI.ViewModels;

public class AddCreditViewModel : BaseViewModel
{
    private readonly AccountStore _accountStore;
    private readonly IBidService _bidService;
    public Account? account => _accountStore.CurrentAccount;
    private decimal _creditAmount;
    public decimal CreditAmount
    {
        get => _creditAmount;
        set
        {
            _creditAmount = value;
            OnPropertyChanged(nameof(CreditAmount));
            OnPropertyChanged(nameof(CanAdd));
        }
    }
    public decimal TotalCredit => account != null ? account.Credit : 0;
    public decimal BidBalance
    {
        get
        {
            decimal bidBalance = 0;
            if (_accountStore.CurrentAccount != null)
            {
                bidBalance = _bidService.GetCulmulativeBidAmountByAccountId(_accountStore.CurrentAccount.AccountId);
            }
            return bidBalance;
        }
    }
    public decimal BiddableCredit => account != null ? TotalCredit - BidBalance : 0;
    public ICommand AddCreditCommand { get; }
    public ICommand CloseModalCommand { get; }
    public bool CanAdd => CreditAmount > 0 && CreditAmount < 99999;
    public AddCreditViewModel(AccountStore accountStore,
        INavigationService closeModalNavigationService,
        IAccountService accountService, IBidService bidService)
    {
        _accountStore = accountStore;
        _bidService = bidService;

        AddCreditCommand = new AddCreditCommand(this, accountStore, closeModalNavigationService, accountService);
        CloseModalCommand = new CloseModalCommand(closeModalNavigationService);
    }
    public void Refresh()
    {
        OnPropertyChanged(nameof(TotalCredit));
        OnPropertyChanged(nameof(BidBalance));
        OnPropertyChanged(nameof(BiddableCredit));
    }
}

