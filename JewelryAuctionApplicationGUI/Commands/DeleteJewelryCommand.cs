using JewelryAuctionApplicationBLL.Services;
using JewelryAuctionApplicationDAL.Models;
using JewelryAuctionApplicationDAL.Repositories;
using JewelryAuctionApplicationGUI.ViewModels;
using JewelryAuctionApplicationGUI.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace JewelryAuctionApplicationGUI.Commands;

public class DeleteJewelryCommand : BaseCommand
{
    private readonly IJewelryService _jewelryService;
    private readonly StaffJewelryManagementViewModel _viewModel;
    public DeleteJewelryCommand(StaffJewelryManagementViewModel staffJewelryManagementViewModel,IJewelryService jewelryService)
    {
        _jewelryService = jewelryService;
        _viewModel = staffJewelryManagementViewModel;
    }

    public override void Execute(object parameter)
    {
        _viewModel.SelectedJewelryAuctionPair.Jewelry.Status = JewelryStatus.DELETED;
        _jewelryService.Update(_viewModel.SelectedJewelryAuctionPair.Jewelry);
        MessageBox.Show("jewelry deleted");
    }
}
