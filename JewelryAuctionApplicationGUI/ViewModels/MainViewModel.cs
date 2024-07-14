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
        //InitializeAuctionTimer();
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
    /*private void InitializeAuctionTimer()
    {
        auctionTimer = new Timer(async _ => await CheckAuctionsAsync(), null, TimeSpan.Zero, TimeSpan.FromSeconds(1));
    }
    private async Task CheckAuctionsAsync()
    {
        var auctions = await _auctionService.GetAllLatest();
        foreach (var auction in auctions)
        {
            if (auction.EndDate < DateTime.Now)
            {
                var jewelry = await _jewelryService.GetById(auction.JewelryId);
                if (jewelry != null && jewelry.Status == JewelryStatus.ACTIVE)
                {
                    if (auction.Bids.Count > 0)
                    {
                        var highestBid = await _bidService.GetHighestBid(auction.AuctionId);
                        highestBid.Account.Credit -= highestBid.BidAmount;
                        await _accountService.Update(highestBid.Account);
                        jewelry.Status = JewelryStatus.SOLD;
                    }
                    else
                    {
                        jewelry.Status = JewelryStatus.READY;
                    }
                    await _jewelryService.Update(jewelry);
                }
            }
        }
    }*/
}
