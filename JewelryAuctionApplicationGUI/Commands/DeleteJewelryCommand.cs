using JewelryAuctionApplicationBLL.Services;
using JewelryAuctionApplicationGUI.ViewModels;
using JewelryAuctionApplicationDAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JewelryAuctionApplicationGUI.Navigation;
using System.Windows;
using MessageBox1 = System.Windows.Forms.MessageBox;
using MessageBox2 = System.Windows.MessageBox;
using System.Windows.Forms;


namespace JewelryAuctionApplicationGUI.Commands;

public class DeleteJewelryCommand : BaseCommand
{
    private readonly IJewelryService _jewelryService;
    private readonly JewelryManagementViewModel _viewModel;
    private readonly INavigationService _navigationService;
    public DeleteJewelryCommand(JewelryManagementViewModel viewModel,
        IJewelryService jewelryService,
        INavigationService returnJewelryManagementNavigationService)
    {
        _jewelryService = jewelryService;
        _viewModel = viewModel;
        _navigationService = returnJewelryManagementNavigationService;
    }
    public override void Execute(object parameter)
    {
        var jewelry = _viewModel.SelectedJewelryListing?.Jewelry;
        if (jewelry?.Status == JewelryStatus.READY || jewelry?.Status == JewelryStatus.SOLD)
        {
            DialogResult result = MessageBox1.Show("Do you want to delete this jewelry?", "Confirmation", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                jewelry.Status = JewelryStatus.DELETED;
                _jewelryService.Update(jewelry);
                _navigationService.Navigate();
                MessageBox2.Show("Jewelry deleted successfully", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }            
        } else if (jewelry?.Status == JewelryStatus.ACTIVE)
        {
            MessageBox2.Show("This jewelry is currently on auction and cannot be deleted", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        } else if (jewelry?.Status == JewelryStatus.DELETED)
        {
            MessageBox2.Show("This jewelry has already been deleted", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
