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

public class UpdateImageViewModel : BaseViewModel
{
    public Jewelry Jewelry { get; }
    private BitmapImage image;
    public BitmapImage Image
    {
        get
        {
            return image;
        }

        set
        {
            image = value;
            OnPropertyChanged(nameof(Image));
        }
    }
    public ICommand UpdateImageCommand { get; }
    public ICommand UploadImageCommand { get; }
    public ICommand CloseModalCommand { get; }
    public UpdateImageViewModel(Jewelry jewelry, IJewelryService jewelryService,
        INavigationService closeModalNavigationService, 
        INavigationService returnJewelryManagementNavigationService)
    {
        Jewelry = jewelry;
        Image = ByteArrayToBitmapImage(Jewelry.Image);
        UpdateImageCommand = new UpdateImageCommand(this, jewelryService, closeModalNavigationService);
        UploadImageCommand = new UploadImageCommand(null, this);
        CloseModalCommand = new NavigateCommand(closeModalNavigationService);
    }
    private BitmapImage ByteArrayToBitmapImage(byte[] byteArray)
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
