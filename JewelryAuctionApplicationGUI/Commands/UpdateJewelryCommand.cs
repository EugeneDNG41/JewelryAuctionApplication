using JewelryAuctionApplicationBLL.Services;
using JewelryAuctionApplicationDAL.Models;
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

public class UpdateJewelryCommand : BaseCommand
{
    private readonly ViewDetailsViewModel _viewModel;
    private readonly IJewelryService _jewelryService;
    public UpdateJewelryCommand(ViewDetailsViewModel viewModel, IJewelryService jewelryService)
    {
        _viewModel = viewModel;
        _jewelryService = jewelryService;
    }

    public override void Execute(object parameter)

    {
        MemoryStream stream = new MemoryStream();
        BitmapEncoder encoder = new PngBitmapEncoder(); // You can use other encoders like JpegBitmapEncoder, BmpBitmapEncoder, etc.
        encoder.Frames.Add(BitmapFrame.Create(_viewModel.Image));
        encoder.Save(stream);
        _viewModel.JewelryDetails.Jewelry.JewelryName = _viewModel.JewelryName;
        _viewModel.JewelryDetails.Jewelry.Description = _viewModel.Description;
        _viewModel.JewelryDetails.Jewelry.JewelryCategory = (JewelryCategory)_viewModel.Category;
        _viewModel.JewelryDetails.Jewelry.Condition= _viewModel.Condition;
        _viewModel.JewelryDetails.Jewelry.Image = stream.ToArray();
        _jewelryService.Update(_viewModel.JewelryDetails.Jewelry);
        MessageBox.Show("Jewelry updated successfully!");


    }
}
