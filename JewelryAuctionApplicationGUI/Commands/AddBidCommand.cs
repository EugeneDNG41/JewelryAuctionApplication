using JewelryAuctionApplicationBLL.Services;
using JewelryAuctionApplicationBLL.Stores;
using JewelryAuctionApplicationDAL.Models;
using JewelryAuctionApplicationGUI.Navigation;
using JewelryAuctionApplicationGUI.ViewModels;
using System.Windows;
using System.Windows.Input;

namespace JewelryAuctionApplicationGUI.Commands;

public class AddBidCommand : BaseCommand
{
    private readonly JewelryListingViewModel _jewelryListing;
    private readonly IBidService _bidService;
    private readonly AddBidViewModel _viewModel;
    private readonly AccountStore _accountStore;
    private readonly ICommand _closeModalCommand;
    
    public AddBidCommand(AddBidViewModel addBidViewModel, 
        JewelryListingViewModel jewelryListing, 
        IBidService bidService, 
        INavigationService closeModelNavigationService,
        AccountStore accountStore)
    {
        _bidService = bidService;
        _jewelryListing = jewelryListing;
        _closeModalCommand = new NavigateCommand(closeModelNavigationService);
        _viewModel = addBidViewModel;
        _accountStore = accountStore;
        
    }

    public override void Execute(object parameter)
    {
        if (_accountStore.CurrentAccount != null && _accountStore.CurrentAccount.Role != Role.USER)
        {   
            Bid? highestBid = _bidService.GetHighestBid(_jewelryListing.ActiveAuction.AuctionId);         
            if (highestBid != null && _viewModel.SelectedBidAmount < highestBid.BidAmount)
            {
                MessageBox.Show("Bid amount must be higher than the current highest bid");
                return;
            }
            var bid = new Bid
            {
                AuctionId = _jewelryListing.ActiveAuction.AuctionId,
                BidAmount = _viewModel.SelectedBidAmount,
                BidTime = DateTime.Now,
                AccountId = _accountStore.CurrentAccount.AccountId
            };
            _bidService.Add(bid);
            _jewelryListing.UpdateListing();
            
            MessageBox.Show("Bid added successfully");
            _closeModalCommand.Execute(null);
        } else
        {
            MessageBox.Show("You must be logged in as a user to place a bid");
            return;
        }
    }
}
