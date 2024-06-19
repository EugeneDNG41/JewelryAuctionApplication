using JewelryAuctionApplication.ViewModels;

namespace JewelryAuctionApplication.Stores;

public class NavigationStore //acts as mediator facilitate communication between view models
{
    private BaseViewModel _currentViewModel; //store the current view model in a centralized state
    public BaseViewModel CurrentViewModel
    {
        get => _currentViewModel;
        set
        {
            _currentViewModel?.Dispose();
            _currentViewModel = value;
            OnCurrentViewModelChanged(); //notify when the view model changes
        }
    }

    public event Action CurrentViewModelChanged;

    private void OnCurrentViewModelChanged()
    {
        CurrentViewModelChanged?.Invoke(); //if current view model != null
    }
}
