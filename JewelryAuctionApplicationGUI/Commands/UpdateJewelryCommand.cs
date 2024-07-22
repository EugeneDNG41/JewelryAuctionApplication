using JewelryAuctionApplicationBLL.Services;
using JewelryAuctionApplicationDAL.Models;
using JewelryAuctionApplicationGUI.Navigation;
using JewelryAuctionApplicationGUI.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace JewelryAuctionApplicationGUI.Commands;

public class UpdateJewelryCommand : BaseCommand
{
    private readonly UpdateJewelryViewModel _viewModel;
    private readonly IJewelryService _jewelryService;
    private readonly INavigationService _navigationService;
    public UpdateJewelryCommand(UpdateJewelryViewModel viewModel, 
        IJewelryService jewelryService, 
        INavigationService returnJewelryManagementNavigationService)
    {
        _viewModel = viewModel;
        _jewelryService = jewelryService;
        _navigationService = returnJewelryManagementNavigationService;
    }
    public override void Execute(object parameter)
    {
        _viewModel.Jewelry.JewelryName = _viewModel.JewelryName;
        _viewModel.Jewelry.Description = _viewModel.Description;
        _viewModel.Jewelry.Condition = _viewModel.Condition;
        _viewModel.Jewelry.JewelryCategory = (JewelryCategory)_viewModel.Category;
        if (_viewModel.Editable)
        {
            _viewModel.Jewelry.StartingPrice = _viewModel.StartingPrice;
            _viewModel.Jewelry.Status = (JewelryStatus)(_viewModel.Status == 0 ? 0 : 3);
        }
        _jewelryService.Update(_viewModel.Jewelry);
        _navigationService.Navigate();
        MessageBox.Show("Jewelry updated successfully", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
    }
}
