using JewelryAuctionApplicationBLL.Services;
using JewelryAuctionApplicationBLL.Stores;
using JewelryAuctionApplicationDAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JewelryAuctionApplicationGUI.ViewModels;

public class AccountInformationViewModel : BaseViewModel
{
    private readonly IAccountService _accountService;
    private readonly IAuctionService _auctionService;
    private readonly IBidService _bidService;
    public Account account;
    public string Status { get; private set; }
    public decimal BidBalance { get; private set; }
    public decimal BiddableCredit { get; private set; }
    public decimal SpentCredit { get; private set; }
    public int WonAuctions { get; private set; }
    public AccountInformationViewModel(Account account, IAccountService accountService, IAuctionService auctionService, IBidService bidService)
    {
        this.account = account;
        _accountService = accountService;
        _auctionService = auctionService;
        _bidService = bidService;
        InitializeAdditionalInformation();
    }

    private void InitializeAdditionalInformation()
    {
        Status = account.Status ? "Active" : "Deleted";
        BidBalance = _bidService.GetCulmulativeBidAmountByAccountId(account.AccountId);
        BiddableCredit = account.Credit - BidBalance;
        var wonAuctions = _auctionService.GetWonAuction(account.AccountId);
        WonAuctions = wonAuctions.Count();
        SpentCredit = 0;
        foreach (var auction in wonAuctions)
        {
            SpentCredit += auction.Bids.Max(b => b.BidAmount);
        }
    }
}
