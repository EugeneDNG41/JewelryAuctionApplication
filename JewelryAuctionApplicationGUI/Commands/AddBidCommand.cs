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
    private readonly IAuctionService _auctionService;
    private readonly IBidService _bidService;
    private readonly AddBidViewModel _viewModel;
    private readonly AccountStore _accountStore;
    private readonly INavigationService _navigationService;
    
    public AddBidCommand(AddBidViewModel addBidViewModel, 
        JewelryListingViewModel jewelryListing, 
        IAuctionService auctionService ,IBidService bidService, 
        INavigationService closeModelNavigationService,
        AccountStore accountStore)
    {
        _auctionService = auctionService;
        _bidService = bidService;
        _jewelryListing = jewelryListing;
        _navigationService = closeModelNavigationService;
        _viewModel = addBidViewModel;
        _accountStore = accountStore;
        
    }

    public override void Execute(object parameter)
    {
        if (_accountStore.CurrentAccount != null && _accountStore.IsUser)
        {   
            var highestBid = _bidService.GetHighestBid(_jewelryListing.LatestAuction.AuctionId);         
            if (highestBid != null && _viewModel.SelectedBidAmount < highestBid.BidAmount)
            {
                MessageBox.Show("Bid amount must be higher than the current highest bid");
                return;
            }
            var bid = new Bid
            {
                AuctionId = _jewelryListing.LatestAuction.AuctionId,
                BidAmount = _viewModel.SelectedBidAmount,
                BidTime = DateTime.Now,
                Account = _accountStore.CurrentAccount,
                AccountId = _accountStore.CurrentAccount.AccountId
                
            };
            _bidService.Add(bid);
            MessageBox.Show("Bid added successfully");
            _navigationService.Navigate();
            return;
        } else
        {
            MessageBox.Show("You must be logged in as a user to place a bid");
            return;
        }
    }
}
