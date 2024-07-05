using JewelryAuctionApplicationBLL.Services;
using JewelryAuctionApplicationDAL.Models;
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
    public AddAuctionCommand(AddAuctionViewModel viewModel, 
        IAuctionService auctionService, 
        IJewelryService jewelryService)
    {
        _viewModel = viewModel;
        _auctionService = auctionService;
        _jewelryService = jewelryService;
    }
    public override void Execute(object parameter)
    {
        if (_viewModel.EndDate < DateTime.Now)
        {
            _viewModel.AddError("End date must be in the future!", nameof(_viewModel.EndDate));
            return;
        }
        if (_viewModel.CurrentPrice <= 0)
        {
            _viewModel.AddError("Current price must be greater than 0!", nameof(_viewModel.CurrentPrice));
            return;
        }
        if (_viewModel.Jewelry == null)
        {
            _viewModel.AddError("Jewelry must be selected!", nameof(_viewModel.Jewelry));
            return;
        }
        
        var auction = new Auction
        {
            CurrentPrice = _viewModel.CurrentPrice,
            EndDate = _viewModel.EndDate,
            JewelryId = _viewModel.Jewelry.JewelryId,
            Status = AuctionStatus.LIVE
        };
        _viewModel.Jewelry.Status = JewelryStatus.ACTIVE;
        _jewelryService.Update(_viewModel.Jewelry);
        _auctionService.Add(auction);
        MessageBox.Show("Auction added successfully!");
    }
}
