using JewelryAuctionApplicationBLL.Services;
using JewelryAuctionApplicationDAL.Models;
using JewelryAuctionApplicationGUI.Navigation;
using System.Windows.Threading;


namespace JewelryAuctionApplicationGUI.ViewModels;

public class MainViewModel : BaseViewModel
{
    private readonly NavigationStore _navigationStore;
    private readonly ModalNavigationStore _modalNavigationStore;
    private readonly IJewelryService _jewelryService;
    private readonly IAuctionService _auctionService;
    private DispatcherTimer auctionTimer;

    public BaseViewModel? CurrentViewModel => _navigationStore.CurrentViewModel; //determine the the view for the application by going through datatemplate that maps view models to views
    public BaseViewModel? CurrentModalViewModel => _modalNavigationStore.CurrentViewModel;
    public bool IsOpen => _modalNavigationStore.IsOpen;

    public MainViewModel(NavigationStore navigationStore, ModalNavigationStore modalNavigationStore, 
        IJewelryService jewelryService, IAuctionService auctionService)
    {
        _navigationStore = navigationStore;
        _modalNavigationStore = modalNavigationStore;
        _jewelryService = jewelryService;
        _auctionService = auctionService;
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
        auctionTimer = new DispatcherTimer();
        auctionTimer.Interval = TimeSpan.FromSeconds(1);
        auctionTimer.Tick += Tick;
        auctionTimer.Start();
    }
    private void Tick(object sender, EventArgs e)
    {
        //CheckAuctions();
    }
    private void CheckAuctions()
    {
        var auctions = _auctionService.GetAllLatest();
        foreach (var auction in auctions)
        {
            if (auction.EndDate < DateTime.Now)
            {
                var jewelry = _jewelryService.GetById(auction.JewelryId);
                if (jewelry != null && jewelry.Status == JewelryStatus.ACTIVE)
                {
                    if (auction.Bids.Count > 0)
                    {
                        jewelry.Status = JewelryStatus.SOLD;                                             
                    }
                    else
                    {
                        jewelry.Status = JewelryStatus.READY;                       
                    }
                    _jewelryService.Update(jewelry);
                }
            }
        }
    }
}
