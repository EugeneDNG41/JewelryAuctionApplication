using JewelryAuctionApplicationGUI.ViewModels;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace JewelryAuctionApplicationGUI.Commands;

public class UploadImageCommand : BaseCommand
{
    private readonly AddJewelryViewModel? _addJewelryviewModel;
    private readonly UpdateImageViewModel? _updateImageViewModel;
    public UploadImageCommand(AddJewelryViewModel? addJewelryViewModel, UpdateImageViewModel? updateImageViewModel)
    {
        _addJewelryviewModel = addJewelryViewModel;
        _updateImageViewModel = updateImageViewModel;
    }
    public override void Execute(object parameter)
    {
        var openFileDialog = new OpenFileDialog
        {
            Filter = "Image files (*.jpg, *.jpeg) | *.jpg; *.jpeg;"
        };

        if (openFileDialog.ShowDialog() == DialogResult.OK)
        {
            if (_addJewelryviewModel != null)
            {
                _addJewelryviewModel.Image = new BitmapImage(new Uri(openFileDialog.FileName));
            } else if (_updateImageViewModel != null)
            {
                _updateImageViewModel.Image = new BitmapImage(new Uri(openFileDialog.FileName));
            }
            else
            {
                MessageBox.Show("No View Model Found", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
        }
    }
}
