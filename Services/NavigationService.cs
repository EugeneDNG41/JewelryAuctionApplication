using JewelryAuctionApplication.Services;
using JewelryAuctionApplication.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JewelryAuctionApplication.Stores;

namespace JewelryAuctionApplication.Services;

public class NavigationService<TViewModel> : INavigationService where TViewModel : BaseViewModel //constrain TViewModel so that it must inherit BaseViewModel
{
    private readonly NavigationStore _navigationStore;
    private readonly Func<TViewModel> _createViewModel;

    public NavigationService(NavigationStore navigationStore, Func<TViewModel> createViewModel)
    {
        _navigationStore = navigationStore;
        _createViewModel = createViewModel;
    }

    public void Navigate()
    {
        _navigationStore.CurrentViewModel = _createViewModel();
    }
}
