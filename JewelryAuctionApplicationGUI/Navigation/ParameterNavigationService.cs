using JewelryAuctionApplicationGUI.ViewModels;


namespace JewelryAuctionApplicationGUI.Navigation;

public class ParameterNavigationService<TParameter, TViewModel> where TViewModel : BaseViewModel
{
    private readonly NavigationStore _navigationStore;
    private readonly Func<TParameter, TViewModel> _createViewModel;

    public ParameterNavigationService(NavigationStore navigationStore, Func<TParameter, TViewModel> createViewModel)
    {
        _navigationStore = navigationStore;
        _createViewModel = createViewModel;
    }

    public void Navigate(TParameter parameter)
    {
        _navigationStore.CurrentViewModel = _createViewModel(parameter);
    }
}
