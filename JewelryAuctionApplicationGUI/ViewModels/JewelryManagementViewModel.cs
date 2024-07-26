using JewelryAuctionApplicationBLL.Services;
using JewelryAuctionApplicationBLL.Stores;
using JewelryAuctionApplicationDAL.Models;
using JewelryAuctionApplicationGUI.Commands;
using JewelryAuctionApplicationGUI.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;

namespace JewelryAuctionApplicationGUI.ViewModels;

public class JewelryManagementViewModel : BaseViewModel
{
    private readonly AccountStore _accountStore;
    public ObservableCollection<JewelryListingViewModel> JewelryListings { get; private set; }
    public ICollectionView JewelryListingCollectionView { get; private set; }
    private string _jewelryNameFilter = string.Empty;
    public string JewelryNameFilter
    {
        get => _jewelryNameFilter;
        set
        {
            _jewelryNameFilter = value;
            OnPropertyChanged(nameof(JewelryNameFilter));
            JewelryListingCollectionView.Refresh();
        }
    }
    private string _jewelryConditionFilter = string.Empty;
    public string JewelryConditionFilter
    {
        get => _jewelryConditionFilter;
        set
        {
            _jewelryConditionFilter = value;
            OnPropertyChanged(nameof(JewelryConditionFilter));
            JewelryListingCollectionView.Refresh();
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
            JewelryListingCollectionView.Refresh();
        }
    }
    private int _jewelryStatusFilter;
    public int JewelryStatusFilter
    {
        get => _jewelryStatusFilter;
        set
        {
            _jewelryStatusFilter = value;
            OnPropertyChanged(nameof(JewelryStatusFilter));
            JewelryListingCollectionView.Refresh();
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
    private JewelryListingViewModel? _selectedJewelryListing;
    public JewelryListingViewModel? SelectedJewelryListing
    {
        get => _selectedJewelryListing;
        set
        {
            _selectedJewelryListing = value ?? null;
            OnPropertyChanged(nameof(SelectedJewelryListing));
            OnPropertyChanged(nameof(CanUpdate));
            OnPropertyChanged(nameof(CanAddAuction));
            UpdateButtons();
        }
    }
    public ObservableCollection<string> Categories =>
        new ObservableCollection<string>(GenerateCategoryList());
    public ObservableCollection<string> Statuses =>
        new ObservableCollection<string>(GenerateStatusList());
    public ObservableCollection<string> SortOptions =>
        new ObservableCollection<string> { "All", "Starting Price", "Current Price", "Number of Bids", "Auction Time", "Number of Auctions" };
    public ObservableCollection<string> SortOrder =>
        new ObservableCollection<string> { "Default", "Ascending", "Descending" };
    public ICommand NavigateAddJewelryCommand { get; }
    public ICommand NavigateUpdateJewelryCommand { get; private set; }
    public ICommand NavigateAddAuctionCommand { get; private set; }
    public ICommand DeleteJewelryCommand { get; }
    private readonly ParameterNavigationService<Jewelry, AddAuctionViewModel> _addAuctionNavigationService;
    private readonly ParameterNavigationService<JewelryListingViewModel, UpdateJewelryViewModel> _updateJewelryNavigationService;  

    public bool CanUpdate => SelectedJewelryListing != null;
    public bool CanAddAuction => SelectedJewelryListing?.Jewelry.Status == JewelryStatus.READY;
    public bool IsAdminOrManager => _accountStore.CurrentAccount?.Role == Role.ADMIN || _accountStore.CurrentAccount?.Role == Role.MANAGER;

    public JewelryManagementViewModel(AccountStore accountStore,
        IJewelryService jewelryService,
        INavigationService addJewelryNavigationService,
        INavigationService returnJewelryManagementNavigationService,
        ParameterNavigationService<JewelryListingViewModel, JewelryPageViewModel> jewelryPageNavigationService,
        ParameterNavigationService<JewelryListingViewModel, UpdateJewelryViewModel> updateJewelryNavigationService,
        ParameterNavigationService<Jewelry, AddAuctionViewModel> addAuctionNavigationService
        )
    {
        _accountStore = accountStore;
        _updateJewelryNavigationService = updateJewelryNavigationService;
        _addAuctionNavigationService = addAuctionNavigationService;
        InitializeJewelryList(jewelryService, jewelryPageNavigationService);
        JewelryListingCollectionView = CollectionViewSource.GetDefaultView(JewelryListings);
        JewelryListingCollectionView.Filter = FilterJewelry;
        NavigateAddJewelryCommand = new NavigateCommand(addJewelryNavigationService);
        DeleteJewelryCommand = new DeleteJewelryCommand(this, jewelryService, returnJewelryManagementNavigationService);
        UpdateButtons();
    }
    private void UpdateButtons()
    {
        if (SelectedJewelryListing != null)
        {
            NavigateUpdateJewelryCommand = new NavigateUpdateJewelryCommand(SelectedJewelryListing, _updateJewelryNavigationService);
            if (SelectedJewelryListing.Jewelry.Status == JewelryStatus.READY)
            {
                NavigateAddAuctionCommand = new NavigateAddAuctionCommand(SelectedJewelryListing.Jewelry, _addAuctionNavigationService);
            }
        }
        OnPropertyChanged(nameof(NavigateUpdateJewelryCommand));
        OnPropertyChanged(nameof(NavigateAddAuctionCommand));
    }
    private void InitializeJewelryList(IJewelryService jewelryService,
        ParameterNavigationService<JewelryListingViewModel, JewelryPageViewModel> navigateJewelryPageService)
    {
        var jewelries = jewelryService.GetAll();
        JewelryListings = new ObservableCollection<JewelryListingViewModel>();
        foreach (var jewelry in jewelries)
        {
            if (jewelry.Auctions.Any())
            {
                var latestAuction = jewelry.Auctions.OrderByDescending(a => a.AuctionId).First();
                var jewelryManagerViewModel = new JewelryListingViewModel(jewelry, latestAuction, navigateJewelryPageService);
                JewelryListings.Add(jewelryManagerViewModel);
            }
            else
            {
                var jewelryManagerViewModel = new JewelryListingViewModel(jewelry, null, navigateJewelryPageService);
                JewelryListings.Add(jewelryManagerViewModel);
            }
        }
    }
    private bool FilterJewelry(object obj)
    {
        if (obj is JewelryListingViewModel jewelryListingViewModel)
        {           
            bool matchesName = jewelryListingViewModel.Jewelry.JewelryName.Contains(JewelryNameFilter, StringComparison.InvariantCultureIgnoreCase);
            bool matchesCondition = jewelryListingViewModel.Jewelry.Condition.Contains(JewelryConditionFilter, StringComparison.InvariantCultureIgnoreCase);
            bool matchesCategory = JewelryCategoryFilter == 0 || jewelryListingViewModel.Jewelry.JewelryCategory == (JewelryCategory)(JewelryCategoryFilter - 1);
            bool matchesStatus = JewelryStatusFilter == 0 || jewelryListingViewModel.Jewelry.Status == (JewelryStatus)(JewelryStatusFilter - 1);
            return matchesCategory && matchesName && matchesStatus && matchesCondition;
        }
        else { return false; }
    }

    private void UpdateSorting()
    {
        JewelryListingCollectionView.SortDescriptions.Clear();
        JewelryListingCollectionView.Refresh();
        if (SelectedSortOption != 0 && SelectedSortOrder != 0)
        {
            var direction = SelectedSortOrder == 1 ? ListSortDirection.Ascending : ListSortDirection.Descending;
            switch (SelectedSortOption)
            {
                case 1:
                    JewelryListingCollectionView.SortDescriptions.Add(new SortDescription("Jewelry.StartingPrice", direction));
                    break;
                case 2:
                    JewelryListingCollectionView.SortDescriptions.Add(new SortDescription("LatestAuction.CurrentPrice", direction));
                    break;
                case 3:
                    JewelryListingCollectionView.SortDescriptions.Add(new SortDescription("LatestAuction.Bids.Count", direction));
                    break;
                case 4:
                    JewelryListingCollectionView.SortDescriptions.Add(new SortDescription("LatestAuction.EndDate", direction));
                    break;
                default:

                    break;
            }
        }
    }
    private List<string> GenerateCategoryList()
    {
        var categories = new List<string>();
        categories.Add("All");
        foreach (JewelryCategory category in Enum.GetValues(typeof(JewelryCategory)))
        {
            string categoryString = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(category.ToString().Replace("_", " and ").ToLower());
            categoryString = categoryString.Replace(" And ", " and ");
            categories.Add(categoryString);
        }
        return categories;
    }
    private List<string> GenerateStatusList()
    {
        var categories = new List<string>();
        categories.Add("All");
        foreach (JewelryStatus status in Enum.GetValues(typeof(JewelryStatus)))
        {
            string categoryString = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(status.ToString().ToLower());
            categories.Add(categoryString);
        }
        return categories;
    }
}
