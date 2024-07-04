using JewelryAuctionApplicationBLL.Services;
using JewelryAuctionApplicationDAL.Models;
using JewelryAuctionApplicationGUI.Navigation;
using JewelryAuctionApplicationGUI.ViewModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace JewelryAuctionApplicationGUI.Commands;

public class AddJewelryCommand : BaseCommand
{
    private readonly AddJewelryViewModel _viewModel;
    private readonly IJewelryService _jewelryService;
    private readonly INavigationService _navigationService;
    public AddJewelryCommand(AddJewelryViewModel viewModel, IJewelryService jewelryService, INavigationService navigationService)
    {
        _viewModel = viewModel;
        _jewelryService = jewelryService;
        _navigationService = navigationService;
    }
    public override void Execute(object parameter)
    {
        if (string.IsNullOrEmpty(_viewModel.JewelryName))
        {
            _viewModel.AddError("Required", nameof(_viewModel.JewelryName));
            return;
        }
        if (string.IsNullOrEmpty(_viewModel.Description))
        {
            _viewModel.AddError("Required", nameof(_viewModel.Description));
            return;
        }
        if (string.IsNullOrEmpty(_viewModel.Condition))
        {
            _viewModel.AddError("Required", nameof(_viewModel.Condition));
            return;
        }
        if (_viewModel.Image == null)
        {
            _viewModel.AddError("Required", nameof(_viewModel.Image));
            return;
        }
        // Convert the BitmapImage to a byte array
        MemoryStream stream = new MemoryStream();
        BitmapEncoder encoder = new PngBitmapEncoder(); // You can use other encoders like JpegBitmapEncoder, BmpBitmapEncoder, etc.
        encoder.Frames.Add(BitmapFrame.Create(_viewModel.Image));
        encoder.Save(stream);
        var jewelry = new Jewelry
        {
            JewelryName = _viewModel.JewelryName,
            Description = _viewModel.Description,
            Condition = _viewModel.Condition,
            StartingPrice = _viewModel.StartingPrice,
            JewelryCategory = JewelryCategory.RINGS,
            Status = true,
            // Convert the memory stream to a byte array
            Image = stream.ToArray()
        };
        _jewelryService.Add(jewelry);
        
    }
}
