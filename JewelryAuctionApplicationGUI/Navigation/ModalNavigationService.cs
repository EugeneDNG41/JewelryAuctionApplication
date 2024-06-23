using JewelryAuctionApplicationBLL.Services;
using JewelryAuctionApplicationBLL.Stores;
using JewelryAuctionApplicationGUI.ViewModels;

namespace JewelryAuctionApplicationGUI.Navigation;

public class ModalNavigationService<TViewModel> : INavigationService
        where TViewModel : BaseViewModel
{
    private readonly ModalNavigationStore _navigationStore;
    private readonly Func<TViewModel> _createViewModel;

    public ModalNavigationService(ModalNavigationStore modalNavigationStore, Func<TViewModel> createViewModel)
    {
        _navigationStore = modalNavigationStore;
        _createViewModel = createViewModel;
    }

    public void Navigate()
    {
        _navigationStore.CurrentViewModel = _createViewModel();
    }
}
