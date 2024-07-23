using JewelryAuctionApplicationBLL.Services;
using JewelryAuctionApplicationDAL.Models;
using JewelryAuctionApplicationGUI.Navigation;
using JewelryAuctionApplicationGUI.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace JewelryAuctionApplicationGUI.Commands;

public class UpdateImageCommand : BaseCommand
{
    private readonly UpdateImageViewModel _viewModel;
    private readonly IJewelryService _jewelryService;
    private readonly INavigationService _navigationService;
    public UpdateImageCommand(UpdateImageViewModel viewModel,
        IJewelryService jewelryService,
        INavigationService returnJewelryManagementNavigationService)
    {
        _viewModel = viewModel;
        _jewelryService = jewelryService;
        _navigationService = returnJewelryManagementNavigationService;
    }
    public override void Execute(object parameter)
    {
        MemoryStream stream = new MemoryStream();
        BitmapEncoder encoder = new PngBitmapEncoder(); // You can use other encoders like JpegBitmapEncoder, BmpBitmapEncoder, etc.
        encoder.Frames.Add(BitmapFrame.Create(_viewModel.Image));
        encoder.Save(stream);
        _viewModel.Jewelry.Image = stream.ToArray();
        _jewelryService.Update(_viewModel.Jewelry);
        MessageBox.Show("Jewelry image updated successfully", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        _navigationService.Navigate();
    }
}
