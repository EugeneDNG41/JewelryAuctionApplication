using JewelryAuctionApplicationBLL.Services;
using JewelryAuctionApplicationBLL.Stores;
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
        }
    }
    public ICommand AddBidCommand { get; }
    public ICommand CloseModalCommand { get; }
    public AddBidViewModel(JewelryListingViewModel jewelryListing, 
        IBidService bidService, 
        INavigationService closeModalNavigationService,
        AccountStore accountStore)
    {
        BidAmounts = GenerateBidAmounts(jewelryListing.ActiveAuction.CurrentPrice);
        AddBidCommand = new AddBidCommand(this, jewelryListing, bidService, closeModalNavigationService, accountStore);
        CloseModalCommand = new CloseModalCommand(closeModalNavigationService);       
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
