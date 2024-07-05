using JewelryAuctionApplicationBLL.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JewelryAuctionApplicationGUI.ViewModels;

public class JewelryPageViewModel : BaseViewModel
{
    public JewelryListingViewModel JewelryListing { get; }
    public NavigationBarViewModel NavigationBarViewModel { get; }

    public JewelryPageViewModel(JewelryListingViewModel jewelryListing, IBidService bidService, NavigationBarViewModel navigationBarViewModel)
    {
        JewelryListing = jewelryListing;
        NavigationBarViewModel = navigationBarViewModel;
    }
}
