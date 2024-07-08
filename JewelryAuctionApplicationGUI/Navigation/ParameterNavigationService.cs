using JewelryAuctionApplicationGUI.ViewModels;


namespace JewelryAuctionApplicationGUI.Navigation;

public class ParameterNavigationService<TParameter, TViewModel> where TViewModel : BaseViewModel
{
    private readonly NavigationStore? _navigationStore;
    private readonly ModalNavigationStore? _modalNavigationStore;
    private readonly Func<TParameter, TViewModel> _createViewModel;

    public ParameterNavigationService(NavigationStore? navigationStore, ModalNavigationStore? modalNavigationStore, Func<TParameter, TViewModel> createViewModel)
    {
        _navigationStore = navigationStore;
        _modalNavigationStore = modalNavigationStore;
        _createViewModel = createViewModel;
    }

    public void Navigate(TParameter parameter)
    {
        if (_navigationStore != null)
        {
           _navigationStore.CurrentViewModel = _createViewModel(parameter);
        } else if (_modalNavigationStore != null)
        {
            _modalNavigationStore.CurrentViewModel = _createViewModel(parameter);
        }

    }
}
