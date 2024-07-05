
using JewelryAuctionApplicationBLL.Services;
using JewelryAuctionApplicationDAL.Models;
using JewelryAuctionApplicationGUI.Commands;
using JewelryAuctionApplicationGUI.Navigation;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Forms.Design;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace JewelryAuctionApplicationGUI.ViewModels;

public class HomeViewModel : BaseViewModel
{

    private readonly ObservableCollection<JewelryListingViewModel> _jewelries;
    private readonly IJewelryService _jewelryService;
    public IEnumerable<JewelryListingViewModel> Jewelries => _jewelries;
    

    public HomeViewModel(
        IJewelryService jewelryService, 
        ParameterNavigationService<JewelryListingViewModel, JewelryPageViewModel> navigateJewelryPageService 
        )
    {        
        _jewelryService = jewelryService;
        _jewelries = new ObservableCollection<JewelryListingViewModel>(_jewelryService.GetAll().Select(j => new JewelryListingViewModel(j, navigateJewelryPageService)));
        
    }
    
}
