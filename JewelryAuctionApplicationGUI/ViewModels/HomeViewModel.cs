
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
    private readonly IAccountService _accountService;
    public ObservableCollection<Account> accounts => new ObservableCollection<Account>(_accountService.GetAll());
    public ObservableCollection<JewelryListingViewModel> _jewelryListings { get; private set; } 
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
        IAuctionService auctionService, IBidService bidService,
        ParameterNavigationService<JewelryListingViewModel, JewelryPageViewModel> navigateJewelryPageService
        )
    {        
        _jewelryService = jewelryService;
        InitializeJewelryList(jewelryService, auctionService, bidService, navigateJewelryPageService);
        JewelryCollectionView = CollectionViewSource.GetDefaultView(_jewelryListings);
        JewelryCollectionView.Filter = FilterJewelry;
    }

    private bool FilterJewelry(object obj)
    {
        if (obj is JewelryListingViewModel jewelryListingViewModel)
        {
            bool matchesCategory = JewelryCategoryFilter == 0 || jewelryListingViewModel.Jewelry.JewelryCategory == (JewelryCategory)(JewelryCategoryFilter - 1);
            bool matchesName = jewelryListingViewModel.Jewelry.JewelryName.Contains(JewelryNameFilter, StringComparison.InvariantCultureIgnoreCase);
            return matchesCategory && matchesName;
        }
        else { return false; }
    }

    private void InitializeJewelryList(
        IJewelryService jewelryService, 
        IAuctionService auctionService, 
        IBidService bidService,
        ParameterNavigationService<JewelryListingViewModel, JewelryPageViewModel> navigateJewelryPageService)
    {
        _jewelryListings = new ObservableCollection<JewelryListingViewModel>(
            _jewelryService.GetJewelriesWithOngoingAuctions()
            .Select(j => new JewelryListingViewModel(j.Jewelry, j.LatestAuction, navigateJewelryPageService, auctionService, bidService, jewelryService)));
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
