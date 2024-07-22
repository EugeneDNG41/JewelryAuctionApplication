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

public class AddAuctionCommand : BaseCommand
{
    private readonly AddAuctionViewModel _viewModel;
    private readonly IAuctionService _auctionService;
    private readonly IJewelryService _jewelryService;
    private readonly INavigationService _navigationService;
    public AddAuctionCommand(AddAuctionViewModel viewModel, 
        IAuctionService auctionService, 
        IJewelryService jewelryService,
        INavigationService returnJewelryManagementNavigationService)
    {
        _viewModel = viewModel;
        _auctionService = auctionService;
        _jewelryService = jewelryService;
        _navigationService = returnJewelryManagementNavigationService;
    }
    public override void Execute(object parameter)
    {
        if (_viewModel.EndDate < DateTime.Now)
        {
            MessageBox.Show("End date must be in the future!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }      
        var auction = new Auction
        {
            CurrentPrice = _viewModel.Jewelry.StartingPrice,
            EndDate = _viewModel.EndDate,
            JewelryId = _viewModel.Jewelry.JewelryId
        };
        _viewModel.Jewelry.Status = JewelryStatus.ACTIVE;
        _jewelryService.Update(_viewModel.Jewelry);
        _auctionService.Add(auction);
        _navigationService.Navigate();
        MessageBox.Show("Auction added successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
    }
}
