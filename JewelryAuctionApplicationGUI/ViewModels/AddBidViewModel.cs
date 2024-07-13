using JewelryAuctionApplicationBLL.Services;
using JewelryAuctionApplicationBLL.Stores;
using JewelryAuctionApplicationDAL.Models;
using JewelryAuctionApplicationGUI.Commands;
using JewelryAuctionApplicationGUI.Navigation;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace JewelryAuctionApplicationGUI.ViewModels;

public class AddBidViewModel : BaseViewModel
{
    private readonly AccountStore _accountStore;
    private readonly IAuctionService _auctionService;
    private readonly INavigationService _closeModalNavigationService;
    private readonly INavigationService _addCreditNavigationService;
    private readonly JewelryListingViewModel _jewelryListing;
    private readonly IBidService _bidService;
    public ObservableCollection<decimal> BidAmounts { get; set; }
    private decimal _selectedBidAmount;
    public decimal SelectedBidAmount
    {
        get
        {
            return _selectedBidAmount;
        }
        set
        {
            _selectedBidAmount = value;
            OnPropertyChanged(nameof(SelectedBidAmount));
            OnPropertyChanged(nameof(InvalidCredit));
            OnPropertyChanged(nameof(ButtonText));
            OnPropertyChanged(nameof(InvalidCreditMessage));
            UpdateButton();
        }
    }
    public bool InvalidCredit => SelectedBidAmount > BiddableCredit;
    public string ButtonText => InvalidCredit ? "Add Credit" : "Add Bid";
    public string InvalidCreditMessage => InvalidCredit? "Insufficient Credit" : "";
    public decimal BiddableCredit
    {
        get
        {
            decimal biddableCredit = 0;
            if (_accountStore.CurrentAccount != null)
            {
                biddableCredit = _accountStore.CurrentAccount.Credit - _bidService.GetCulmulativeBidAmountByAccountId(_accountStore.CurrentAccount.AccountId);
                var currentHighestBid = _bidService.GetHighestBid(_jewelryListing.LatestAuction.AuctionId);
                if (currentHighestBid != null && currentHighestBid.AccountId == _accountStore.CurrentAccount.AccountId)
                {
                    biddableCredit += currentHighestBid.BidAmount;
                }
            }
            return biddableCredit;
        }
    }
    public ICommand AddBidCommand { get; private set; }
    public ICommand CloseModalCommand { get; }
    public AddBidViewModel(JewelryListingViewModel jewelryListing, 
        IAuctionService auctionService, IBidService bidService, 
        INavigationService closeModalNavigationService, INavigationService addCreditNavigationService,
        AccountStore accountStore)
    {
        _jewelryListing = jewelryListing;
        _auctionService = auctionService;
        _bidService = bidService;
        _accountStore = accountStore;
        _closeModalNavigationService = closeModalNavigationService;
        _addCreditNavigationService = addCreditNavigationService;
        BidAmounts = GenerateBidAmounts(jewelryListing.LatestAuction.CurrentPrice);
        _selectedBidAmount = BidAmounts[0];
        CloseModalCommand = new CloseModalCommand(closeModalNavigationService);
        UpdateButton();
    }
    private void UpdateButton()
    {
        if (InvalidCredit)
        {
            AddBidCommand = new NavigateCommand(_addCreditNavigationService);
        }
        else
        {
            AddBidCommand = new AddBidCommand(this, _jewelryListing, _auctionService, _bidService, _closeModalNavigationService, _accountStore);
        }
        OnPropertyChanged(nameof(AddBidCommand));
    }
    private ObservableCollection<decimal> GenerateBidAmounts(decimal currentPrice)
    {
        // Define the bid increments based on the price ranges
        var bidIncrements = new List<(decimal Price, decimal Increment)>
        {
            (0m, 10m),
            (100m, 25m),
            (300m, 50m),
            (1000m, 100m),
            (5000m, 250m),
            (10000m, 500m),
            (20000m, 1000m),
            (30000m, 2000m),
            (50000m, 5000m)
        };
        
        decimal increment = 0m;
        var bidAmounts = new List<decimal>();
        // Determine the appropriate bid increment based on the current price
        for (decimal price = currentPrice + increment; bidAmounts.Count < 10;)
        {
            foreach (var (threshold, inc) in bidIncrements)
            {
                if (price >= threshold)
                {
                    increment = inc;
                }
                else
                {
                    break;
                }
            }

            // Increment price and add to bid amounts list
            price += increment;
            bidAmounts.Add(price);
        }

        // Convert the list to an ObservableCollection
        var bidAmountsCollection = new ObservableCollection<decimal>(bidAmounts);
        return bidAmountsCollection;
    }
}
