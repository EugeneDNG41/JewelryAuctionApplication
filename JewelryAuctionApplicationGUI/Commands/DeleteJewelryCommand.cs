using JewelryAuctionApplicationBLL.Services;
using JewelryAuctionApplicationGUI.ViewModels;
using JewelryAuctionApplicationDAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using JewelryAuctionApplicationGUI.Navigation;

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
        if (jewelry != null && jewelry.Status == JewelryStatus.READY)
        {
            jewelry.Status = JewelryStatus.DELETED;
            _jewelryService.Update(jewelry);
            _navigationService.Navigate();
            MessageBox.Show("Jewelry deleted successfully", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        } else
        {
            MessageBox.Show("Jewelry cannot be deleted", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
