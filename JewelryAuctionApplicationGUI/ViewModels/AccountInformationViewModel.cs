using JewelryAuctionApplicationBLL.Services;
using JewelryAuctionApplicationDAL.Models;


namespace JewelryAuctionApplicationGUI.ViewModels;

public class AccountInformationViewModel : BaseViewModel
{
    private readonly IBidService _bidService;
    public Account Account { get; }
    public string Status { get; private set; }
    public decimal BidBalance { get; private set; }
    public decimal BiddableCredit { get; private set; }
    public decimal SpentCredit { get; private set; }
    public int WonAuctions { get; private set; }
    public AccountInformationViewModel(Account account, IBidService bidService)
    {
        Account = account;
        _bidService = bidService;
        LoadAdditionalInformation();
    }

    private void LoadAdditionalInformation()
    {
        Status = Account.Status ? "Active" : "Deleted";
        BiddableCredit = Account.Credit - BidBalance;
        var wonAuctions = Account.Auctions;
        WonAuctions = wonAuctions.Count;
        SpentCredit = 0;
        foreach (var auction in wonAuctions)
        {
            SpentCredit += auction.CurrentPrice;
        }
    }
}
