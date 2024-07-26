using JewelryAuctionApplicationBLL.Services;
using JewelryAuctionApplicationDAL.Models;
using JewelryAuctionApplicationGUI.Navigation;
using System.Windows.Threading;


namespace JewelryAuctionApplicationGUI.ViewModels;

public class MainViewModel : BaseViewModel
{
    private readonly NavigationStore _navigationStore;
    private readonly ModalNavigationStore _modalNavigationStore;
    public BaseViewModel? CurrentViewModel => _navigationStore.CurrentViewModel;
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
        OnPropertyChanged(nameof(CurrentViewModel));
    }
    private void OnCurrentModalViewModelChanged()
    {
        OnPropertyChanged(nameof(CurrentModalViewModel));
        OnPropertyChanged(nameof(IsOpen));
    }
}
