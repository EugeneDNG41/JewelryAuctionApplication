using JewelryAuctionApplicationBLL.Services;
using JewelryAuctionApplicationDAL.Models;
using JewelryAuctionApplicationGUI.Navigation;
using System.Windows.Threading;


namespace JewelryAuctionApplicationGUI.ViewModels;

public class MainViewModel : BaseViewModel
{
    private readonly NavigationStore _navigationStore;
    private readonly ModalNavigationStore _modalNavigationStore;
    private readonly IAccountService _accountService;
    private readonly IJewelryService _jewelryService;
    private readonly IAuctionService _auctionService;
    private readonly IBidService _bidService;
    private Timer auctionTimer;
    public BaseViewModel? CurrentViewModel => _navigationStore.CurrentViewModel; //determine the the view for the application by going through datatemplate that maps view models to views
    public BaseViewModel? CurrentModalViewModel => _modalNavigationStore.CurrentViewModel;
    public bool IsOpen => _modalNavigationStore.IsOpen;

    public MainViewModel(NavigationStore navigationStore, ModalNavigationStore modalNavigationStore, 
        IAccountService accountService, IJewelryService jewelryService, IAuctionService auctionService, IBidService bidService)
    {
        _navigationStore = navigationStore;
        _modalNavigationStore = modalNavigationStore;
        _accountService = accountService;
        _jewelryService = jewelryService;
        _auctionService = auctionService;
        _bidService = bidService;
        _accountService.CreateAdmin();
        InitializeAuctionTimer();
        _navigationStore.CurrentViewModelChanged += OnCurrentViewModelChanged;
        _modalNavigationStore.CurrentViewModelChanged += OnCurrentModalViewModelChanged;
    }

    private void OnCurrentViewModelChanged()
    {
        OnPropertyChanged(nameof(CurrentViewModel)); //raise property change
    }
    private void OnCurrentModalViewModelChanged()
    {
        OnPropertyChanged(nameof(CurrentModalViewModel));
        OnPropertyChanged(nameof(IsOpen));
    }
    private void InitializeAuctionTimer()
    {
        auctionTimer = new Timer(async _ => await CheckAuctionsAsync(), null, TimeSpan.Zero, TimeSpan.FromSeconds(1));
    }
    private async Task CheckAuctionsAsync()
    {
        var auctions = await _auctionService.GetAllLatest();
        foreach (var auction in auctions)
        {
            if (auction.EndDate < DateTime.Now) //check when auction has ended
            {
                var jewelry = auction.Jewelry;
                if (jewelry != null && jewelry.Status == JewelryStatus.ACTIVE) //but only when jewelry still active = not yet finalized post-auction
                {
                    if (auction.Bids.Any())
                    {
                        var bids = auction.Bids.OrderByDescending(b => b.BidAmount);
                        var winner = new Account();
                        foreach (var bid in bids)
                        {
                            if (bid.Account.Status)
                            {
                                auction.CurrentPrice = bid.BidAmount;
                                winner = bid.Account;
                                break;
                            }
                            auction.CurrentPrice = auction.Jewelry.StartingPrice;
                        }
                        if (auction.CurrentPrice == jewelry.StartingPrice)
                        {
                            jewelry.Status = JewelryStatus.READY;
                        }
                        else
                        {
                            jewelry.Status = JewelryStatus.SOLD;
                            auction.Account = winner;
                            winner.Credit -= auction.CurrentPrice;
                            await _accountService.UpdateAsync(winner);
                            await _auctionService.UpdateAsync(auction);
                        }
                    }
                    else
                    {
                        jewelry.Status = JewelryStatus.READY;
                    }
                    await _jewelryService.UpdateAsync(jewelry);
                }
            }
        }
    }
}
