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
        if (bids.IsNullOrEmpty())
        {
            return null;
        }
        else
        {
            return bids.OrderByDescending(b => b.BidAmount).FirstOrDefault();
        }
    }
    public IEnumerable<Bid> GetByAuctionId(int id)
    {
        return _context.Bids.Where(b => b.AuctionId == id);
    }
}
