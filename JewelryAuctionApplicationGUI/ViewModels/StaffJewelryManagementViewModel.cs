using JewelryAuctionApplicationBLL.Services;
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

public class StaffJewelryManagementViewModel : BaseViewModel
{
    public ObservableCollection<JewelryManagerViewModel> jewelryList { get; private set; }
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
    private JewelryManagerViewModel? _selectedJewelryAuctionPair;
    public JewelryManagerViewModel? SelectedJewelryAuctionPair
    {
        get => _selectedJewelryAuctionPair;
        set
        {
            _selectedJewelryAuctionPair = value ?? null;
            OnPropertyChanged(nameof(SelectedJewelryAuctionPair));
            OnPropertyChanged(nameof(CanClick));
        }
    }
    public ObservableCollection<string> Categories =>
        new ObservableCollection<string>(GenerateCategoryList());
    public ObservableCollection<string> SortOptions =>
        new ObservableCollection<string> { "All", "Price", "Bid Number", "Auction Time" };
    public ObservableCollection<string> SortOrder =>
        new ObservableCollection<string> { "Default", "Ascending", "Descending" };
    public ICommand NavigateViewDetailCommand { get; }
    public ICommand NavigateAddJewelryCommand { get; }
    public ICommand DeleteJewelryCommand { get; }
    public bool CanClick => SelectedJewelryAuctionPair != null;
    private readonly IJewelryService _jewelryService;

    public StaffJewelryManagementViewModel(IJewelryService jewelryService 
        //INavigationService navigateViewDetailCommand,
        //INavigationService navigateAddJewelryCommand
        )
    {
        _jewelryService = jewelryService;
        //NavigateViewDetailCommand = new NavigateCommand(navigateViewDetailCommand);
        //NavigateAddJewelryCommand = new NavigateCommand(navigateAddJewelryCommand);
        DeleteJewelryCommand = new DeleteJewelryCommand(this, jewelryService);
        InitializeJewelryList(jewelryService);
        JewelryCollectionView = CollectionViewSource.GetDefaultView(jewelryList);
        JewelryCollectionView.Filter = FilterJewelryName;
        JewelryCollectionView.Filter = FilterJewelryCategory;
    }
    private void InitializeJewelryList(IJewelryService jewelryService)
    {
        var jewelries = jewelryService.GetAll();
        jewelryList = new ObservableCollection<JewelryManagerViewModel>();
        foreach (var jewelry in jewelries)
        {
            if (jewelry.Auctions.Any())
            {
                var latestAuction = jewelry.Auctions.OrderByDescending(a => a.EndDate).FirstOrDefault();
                var jewelryManagerViewModel = new JewelryManagerViewModel(jewelry, latestAuction);
                jewelryList.Add(jewelryManagerViewModel);
            }
            else
            {
                var jewelryManagerViewModel = new JewelryManagerViewModel(jewelry, null);
                jewelryList.Add(jewelryManagerViewModel);
            }
        }
    }
    private bool FilterJewelryCategory(object obj)
    {
        if (obj is JewelryManagerViewModel jewelryManager)
        {
            if (JewelryCategoryFilter == 0)
            {
                return true;
            }
            else
            {
                return jewelryManager.Jewelry.JewelryCategory == (JewelryCategory)(JewelryCategoryFilter - 1);
            }
        }
        else { return false; }
    }

    private bool FilterJewelryName(object obj)
    {
        if (obj is JewelryManagerViewModel jewelryManager)
        {
            return jewelryManager.Jewelry.JewelryName.Contains(JewelryNameFilter);
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
public class JewelryManagerViewModel : BaseViewModel
{
    public Jewelry Jewelry { get; }
    public Auction? LatestAuction { get; }

    public JewelryManagerViewModel(Jewelry jewelry, Auction? auction)
    {
        Jewelry = jewelry;
        LatestAuction = auction;
    }
}