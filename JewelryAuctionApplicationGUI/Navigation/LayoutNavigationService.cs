
using JewelryAuctionApplicationGUI.ViewModels;


namespace JewelryAuctionApplicationGUI.Navigation;

public class LayoutNavigationService<TViewModel> : INavigationService where TViewModel : BaseViewModel
{
    private readonly NavigationStore _navigationStore;
    private readonly Func<NavigationBarViewModel> _createNavigationBarViewModel;
    private readonly Func<TViewModel> _createViewModel;
    

    public LayoutNavigationService(NavigationStore navigationStore, Func<NavigationBarViewModel> createNavigationBarViewModel, Func<TViewModel> createViewModel)
    {
        _navigationStore = navigationStore;
        _createViewModel = createViewModel;
        _createNavigationBarViewModel = createNavigationBarViewModel;
    }

    public void Navigate()
    {
        _navigationStore.CurrentViewModel = new LayoutViewModel(_createNavigationBarViewModel(), _createViewModel());
    }
}
