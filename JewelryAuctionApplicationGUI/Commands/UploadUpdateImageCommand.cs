using JewelryAuctionApplicationGUI.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media.Imaging;

namespace JewelryAuctionApplicationGUI.Commands;

public class UploadUpdateImageCommand : BaseCommand
{
    private readonly ViewDetailsViewModel _viewModel;
    public UploadUpdateImageCommand(ViewDetailsViewModel viewModel)
    {
        _viewModel = viewModel;
    }
    public override void Execute(object parameter)
    {
        var openFileDialog = new OpenFileDialog
        {
            Filter = "Image files (*.jpg, *.jpeg) | *.jpg; *.jpeg;"
        };

        if (openFileDialog.ShowDialog() == DialogResult.OK)
        {
            _viewModel.Image = new BitmapImage(new Uri(openFileDialog.FileName));
        }
    }
}
