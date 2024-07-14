using JewelryAuctionApplicationDAL.Context;
using JewelryAuctionApplicationDAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JewelryAuctionApplicationDAL.Repositories;

public class BidRepository : IBidRepository
{
    private readonly JewelryAuctionContext _context;

    public BidRepository(JewelryAuctionContext context)
    {
        _context = context;
    }

    public void AddBid(Bid bid)
    {
        _context.Bids.Add(bid);
        _context.SaveChanges();
        
    }

    public IEnumerable<Bid> GetAll()
    {
        return _context.Bids;
    }

    public Bid? GetHighestBid(int auctionId)
    {
        var bids = _context.Bids.Include(b => b.Account).Where(b => b.AuctionId == auctionId);
        return bids.OrderByDescending(b => b.BidAmount).FirstOrDefault();
    }

    public IEnumerable<Bid> GetByAuctionId(int id)
    {
        return _context.Bids.Where(b => b.AuctionId == id);
    }

    public decimal GetCulmulativeBidAmountByAccountId(int id)
    {
        var allBids = _context.Bids
                          .Include(b => b.Auction)
                          .Where(b => b.AccountId == id && b.Auction.EndDate > DateTime.Now)
                          ;

        // Group bids by auction and select the highest bid in each group
        var leadingBids = allBids.GroupBy(b => b.Auction.AuctionId)
                                 .Select(g => g.OrderByDescending(b => b.BidAmount).First());

        // Sum the highest bids
        decimal sum = 0;
        foreach (var bid in leadingBids)
        {
            sum += bid.BidAmount;
        }

        return sum;
    }
}
