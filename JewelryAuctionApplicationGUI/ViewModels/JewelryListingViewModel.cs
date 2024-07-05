using JewelryAuctionApplicationBLL.Services;
using JewelryAuctionApplicationDAL.Models;
using JewelryAuctionApplicationGUI.Commands;
using JewelryAuctionApplicationGUI.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace JewelryAuctionApplicationGUI.ViewModels;

public class JewelryListingViewModel
{
    public Jewelry Jewelry {  get; set; }
    public BitmapImage DisplayedImage => ByteArrayToBitmapImage(Jewelry.Image);
    public ICommand NavigateJewelryPageCommand { get; }
    public JewelryListingViewModel(Jewelry jewelry,
        ParameterNavigationService<JewelryListingViewModel, JewelryPageViewModel> navigateJewelryPageService)
    {
        Jewelry = jewelry;
        NavigateJewelryPageCommand = new NavigateJewelryPageCommand(this, navigateJewelryPageService);
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
