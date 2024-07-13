
using JewelryAuctionApplicationBLL.Services;
using JewelryAuctionApplicationDAL.Models;
using JewelryAuctionApplicationGUI.Commands;
using JewelryAuctionApplicationGUI.Navigation;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Windows.Data;
using System.Windows.Forms.Design;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace JewelryAuctionApplicationGUI.ViewModels;

public class HomeViewModel : BaseViewModel
{
    private readonly IJewelryService _jewelryService;
    private readonly ObservableCollection<JewelryListingViewModel> _jewelries;  
    public ICollectionView JewelryCollectionView { get; private set; }
    private string _jewelryNameFilter = string.Empty;
    public string JewelryNameFilter
    {
        get => _jewelryNameFilter;
        set
        {
            _jewelryNameFilter = value;
            OnPropertyChanged(nameof(JewelryNameFilter));
            JewelryCollectionView.Refresh();
        }
    }
    private int _jewelryCategoryFilter;
    public int JewelryCategoryFilter
    {
        get => _jewelryCategoryFilter;
        set
        {
            _jewelryCategoryFilter = value;
            OnPropertyChanged(nameof(JewelryCategoryFilter));
            JewelryCollectionView.Refresh();
        }
    }
    private int _selectedSortOption;
    public int SelectedSortOption
    {
        get => _selectedSortOption;
        set
        {
            _selectedSortOption = value;
            OnPropertyChanged(nameof(SelectedSortOption));
            UpdateSorting();
        }
    }

    private int _selectedSortOrder;
    public int SelectedSortOrder
    {
        get => _selectedSortOrder;
        set
        {
            _selectedSortOrder = value;
            OnPropertyChanged(nameof(SelectedSortOrder));
            UpdateSorting();
        }
    }
    public ObservableCollection<string> Categories =>
        new ObservableCollection<string>(GenerateCategoryList());
    public ObservableCollection<string> SortOptions => 
        new ObservableCollection<string> { "All", "Price", "Bid Number", "Auction Time" };
    public ObservableCollection<string> SortOrder =>
        new ObservableCollection<string> { "Default", "Ascending", "Descending"};
    public HomeViewModel(
        IJewelryService jewelryService, 
        ParameterNavigationService<JewelryListingViewModel, JewelryPageViewModel> navigateJewelryPageService,
        IAuctionService auctionService, IBidService bidService
        )
    {        
        _jewelryService = jewelryService;
        _jewelries = new ObservableCollection<JewelryListingViewModel>(
            _jewelryService.GetOnAuction().Select(j => new JewelryListingViewModel(j, navigateJewelryPageService, auctionService, bidService, jewelryService)));
        JewelryCollectionView = CollectionViewSource.GetDefaultView(_jewelries);
        JewelryCollectionView.Filter = FilterJewelryName;
        JewelryCollectionView.Filter = FilterJewelryCategory;
        //InitializeJewelriesAsync(navigateJewelryPageService, auctionService, bidService);
    }
    /*private async void InitializeJewelriesAsync(
        ParameterNavigationService<JewelryListingViewModel, JewelryPageViewModel> navigateJewelryPageService,
        IAuctionService auctionService, IBidService bidService)
    {
        var jewelries = await _jewelryService.GetOnAuction();

        foreach (var jewelry in jewelries)
        {
            _jewelries.Add(new JewelryListingViewModel(jewelry, navigateJewelryPageService, auctionService, bidService, _jewelryService));
        }

        JewelryCollectionView = CollectionViewSource.GetDefaultView(_jewelries);
        JewelryCollectionView.Filter = FilterJewelryName;
        JewelryCollectionView.Filter = FilterJewelryCategory;
    }*/

    private bool FilterJewelryCategory(object obj)
    {
        if (obj is JewelryListingViewModel jewelryListingViewModel)
        {
            if (JewelryCategoryFilter == 0)
            {
                return true;
            }
            else
            {
                return jewelryListingViewModel.Jewelry.JewelryCategory == (JewelryCategory)(JewelryCategoryFilter - 1);
            }
        }
        else { return false; }
    }

    private bool FilterJewelryName(object obj)
    {
        if (obj is JewelryListingViewModel jewelryListingViewModel)
        {
            return jewelryListingViewModel.Jewelry.JewelryName.Contains(JewelryNameFilter);
        }
        else { return false; }
    }
    private void UpdateSorting()
    {
        JewelryCollectionView.SortDescriptions.Clear();
        JewelryCollectionView.Refresh();
        if (SelectedSortOption != 0 && SelectedSortOrder != 0)
        {
            var direction = SelectedSortOrder == 1 ? ListSortDirection.Ascending : ListSortDirection.Descending;
            switch (SelectedSortOption)
            {
                case 1:
                    JewelryCollectionView.SortDescriptions.Add(new SortDescription("LatestAuction.CurrentPrice", direction));
                    break;
                case 2:
                    JewelryCollectionView.SortDescriptions.Add(new SortDescription("LatestAuction.Bids.Count", direction));
                    break;
                case 3:
                    JewelryCollectionView.SortDescriptions.Add(new SortDescription("LatestAuction.EndDate", direction));
                    break;
                default:
                    
                    break;
            }
        }
    }
    private List<string> GenerateCategoryList()
    {
        var categories = new List<string>();
        categories.Add("All Categories");
        foreach (JewelryCategory category in Enum.GetValues(typeof(JewelryCategory)))
        {
            string categoryString = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(category.ToString().Replace("_", " and ").ToLower());
            categoryString = categoryString.Replace(" And ", " and ");
            categories.Add(categoryString);
        }
        return categories;
    }
}
