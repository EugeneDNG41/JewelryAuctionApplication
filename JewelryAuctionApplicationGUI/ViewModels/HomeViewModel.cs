
using JewelryAuctionApplicationBLL.Services;
using JewelryAuctionApplicationDAL.Models;
using JewelryAuctionApplicationGUI.Commands;
using JewelryAuctionApplicationGUI.Navigation;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace JewelryAuctionApplicationGUI.ViewModels;

public class HomeViewModel : BaseViewModel
{

    private readonly ObservableCollection<JewelryListing> _jewelries;
    private readonly IJewelryService _jewelryService;
    public IEnumerable<JewelryListing> Jewelries => _jewelries;
    

    public HomeViewModel(INavigationService loginNavigationService, IJewelryService jewelryService)
    {
        _jewelryService = jewelryService;
        _jewelries = new ObservableCollection<JewelryListing>(jewelryService.GetAll().Select(j => new JewelryListing(j)));
        
    }
    
}
public class JewelryListing
{
    private readonly Jewelry _jewelry;
    public BitmapImage Image { get; }
    public string JewelryName => _jewelry.JewelryName;
    public string Condition => _jewelry.Condition;
    public decimal StartingPrice => _jewelry.StartingPrice;
    public JewelryListing(Jewelry jewelry)
    {
        _jewelry = jewelry;
        Image = ByteArrayToBitmapImage(_jewelry.Image);
    }
    public BitmapImage ByteArrayToBitmapImage(byte[] byteArray)
    {
        using (MemoryStream stream = new MemoryStream(byteArray))
        {
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            image.CacheOption = BitmapCacheOption.OnLoad;
            image.StreamSource = stream;
            image.EndInit();
            return image;
        }
    }
}
