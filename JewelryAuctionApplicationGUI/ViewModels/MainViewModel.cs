using JewelryAuctionApplicationBLL.Services;
using JewelryAuctionApplicationDAL.Models;
using JewelryAuctionApplicationGUI.Navigation;
using System.Windows.Threading;


namespace JewelryAuctionApplicationGUI.ViewModels;

public class MainViewModel : BaseViewModel
{
    private readonly NavigationStore _navigationStore;
    private readonly ModalNavigationStore _modalNavigationStore;
    public BaseViewModel? CurrentViewModel => _navigationStore.CurrentViewModel; //determine the the view for the application by going through datatemplate that maps view models to views
    public BaseViewModel? CurrentModalViewModel => _modalNavigationStore.CurrentViewModel;
    public bool IsOpen => _modalNavigationStore.IsOpen;

    public MainViewModel(NavigationStore navigationStore, ModalNavigationStore modalNavigationStore, 
        IAccountService accountService)
    {
        _navigationStore = navigationStore;
        _modalNavigationStore = modalNavigationStore;
        accountService.CreateAdmin();
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
}
