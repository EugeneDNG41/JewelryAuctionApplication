using JewelryAuctionApplicationBLL.Services;
using JewelryAuctionApplicationBLL.Stores;
using JewelryAuctionApplicationDAL.Models;
using JewelryAuctionApplicationGUI.Commands;
using JewelryAuctionApplicationGUI.Navigation;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows.Threading;

namespace JewelryAuctionApplicationGUI.ViewModels;

public class JewelryPageViewModel : BaseViewModel
{
    private DispatcherTimer _auctionTimer;
    private readonly AccountStore _accountStore;
    private readonly IAccountService _accountService;
    private readonly IBidService _bidService;
    private readonly ParameterNavigationService<JewelryListingViewModel, AddBidViewModel> _navigationAddBidCommand;
    private readonly INavigationService _loginNavigationService;
    private readonly Func<NavigationBarViewModel> _createNavigationBarViewModel;
    public JewelryListingViewModel JewelryListing { get; }
    public NavigationBarViewModel NavigationBarViewModel { get; private set; }
    public ObservableCollection<Tuple<string, decimal, string, int>> BidHistory => GetBidHistory();
    public Account? Winner
    {
        get
        {
            var highestBid = _bidService.GetHighestBid(JewelryListing.LatestAuction.AuctionId);
            if (JewelryListing.LatestAuction?.EndDate < DateTime.Now && highestBid != null)
            {
                return highestBid.Account;
            }
            else
            {
                return null;
            }
        }
    }
    private string _tickingTimeLeft;
    public string TickingTimeLeft
    {
        get => _tickingTimeLeft;
        private set
        {
            _tickingTimeLeft = value;
            OnPropertyChanged(nameof(TickingTimeLeft));
        }
    }
    public string BidBoxTitle => GetBidBoxTitle();
    public bool CanBid => JewelryListing.LatestAuction?.EndDate > DateTime.Now;
    public ICommand NavigateAddBidCommand { get; private set; }
    public JewelryPageViewModel(JewelryListingViewModel jewelryListing, 
        IAccountService accountService, IBidService bidService, Func<NavigationBarViewModel> navigationBarViewModel,
        ParameterNavigationService<JewelryListingViewModel, AddBidViewModel> navigateAddBidCommand,
        INavigationService loginNavigationService,
        AccountStore accountStore)
    {
        JewelryListing = jewelryListing;
        _accountService = accountService;
        _bidService = bidService;
        _accountStore = accountStore;
        _navigationAddBidCommand = navigateAddBidCommand;
        _loginNavigationService = loginNavigationService;
        _createNavigationBarViewModel = navigationBarViewModel;
        _accountStore.CurrentAccountChanged += OnCurrentAccountChanged;
        UpdateButtonAndNavBar();
        InitializeTimer();
    }
    private string GetBidBoxTitle()
    {
        int bidCount = JewelryListing.LatestAuction.Bids != null ? JewelryListing.LatestAuction.Bids.Count : 0;
        if (JewelryListing.LatestAuction.EndDate < DateTime.Now && bidCount == 0)
        {
            return "No bids";
        }
        else if (JewelryListing.LatestAuction.EndDate < DateTime.Now && bidCount > 0)
        {
            return $"Winning bid ({JewelryListing.BidNumber})";
        }
        else
        {
            return bidCount > 0 ? $"Current Price ({JewelryListing.BidNumber})" : "Starting Price";
        }
    }

    private ObservableCollection<Tuple<string, decimal, string, int>> GetBidHistory()
    {
        var bids = JewelryListing.LatestAuction.Bids.OrderByDescending(b => b.BidAmount);
        if (bids == null)
        {
            return new ObservableCollection<Tuple<string, decimal, string, int>>();
        }
        else
        {
            var bidHistory = new ObservableCollection<Tuple<string, decimal, string, int>>();
            int i = 1;
            foreach (var bid in bids)
            {
                TimeSpan timeDifference = DateTime.Now.Subtract(bid.BidTime);
                string timeAgo;
                if (timeDifference.Days > 0)
                {
                    timeAgo = $"{timeDifference.Days}d ago";
                }
                else if (timeDifference.Hours > 0)
                {
                    timeAgo = $"{timeDifference.Hours}h ago";
                }
                else if (timeDifference.Minutes > 0)
                {
                    timeAgo = $"{timeDifference.Minutes}m ago";
                }
                else
                {
                    int seconds = Math.Max(0, (int)timeDifference.TotalSeconds);
                    timeAgo = $"{seconds}s ago";
                }
                var account = _accountService.GetById(bid.AccountId);
                if (account != null)
                {
                    bidHistory.Add(new Tuple<string, decimal, string, int>(account.Username, bid.BidAmount, timeAgo, i));
                    i++;
                }
                
            }
            return bidHistory;
        }
    }
    private void OnCurrentAccountChanged()
    {
        UpdateButtonAndNavBar();
        OnPropertyChanged(nameof(NavigateAddBidCommand));
        OnPropertyChanged(nameof(NavigationBarViewModel));
    }
    private void UpdateButtonAndNavBar()
    {
        if (_accountStore.CurrentAccount != null && _accountStore.CurrentAccount.Role == Role.USER)
        {
            NavigateAddBidCommand = new NavigateAddBidCommand(JewelryListing, _navigationAddBidCommand);
        }
        else
        {
            NavigateAddBidCommand = new NavigateCommand(_loginNavigationService);
        }
        NavigationBarViewModel = _createNavigationBarViewModel();
    }
    private void InitializeTimer()
    {
        _auctionTimer = new DispatcherTimer
        {
            Interval = TimeSpan.FromMilliseconds(100)
        };
        _auctionTimer.Tick += AuctionTimer_Tick;
        _auctionTimer.Start();
    }
    private void AuctionTimer_Tick(object sender, EventArgs e)
    {
        if (JewelryListing.LatestAuction.EndDate > DateTime.Now)
        {
            TimeSpan timeDifference = JewelryListing.LatestAuction.EndDate.Subtract(DateTime.Now);
            TickingTimeLeft = $"Ends in {timeDifference.Days}d {timeDifference.Hours}h {timeDifference.Minutes}m {timeDifference.Seconds}s";
            JewelryListing.UpdateCurrentPrice();
            OnPropertyChanged(nameof(BidHistory));
            OnPropertyChanged(nameof(TickingTimeLeft));
            OnPropertyChanged(nameof(BidBoxTitle));
        }
        else if (JewelryListing.LatestAuction?.EndDate < DateTime.Now)
        {
            TickingTimeLeft = "Ended";
            _auctionTimer.Stop();
            OnPropertyChanged(nameof(Winner));
            OnPropertyChanged(nameof(BidBoxTitle));
            OnPropertyChanged(nameof(CanBid));
        }
        else
        {
            TickingTimeLeft = "No auction found";
            _auctionTimer.Stop();
        }
    }
    public override void Dispose()
    {
        _accountStore.CurrentAccountChanged -= OnCurrentAccountChanged;
        base.Dispose();
    }
}
