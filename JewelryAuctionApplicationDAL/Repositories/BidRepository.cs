using JewelryAuctionApplicationDAL.Context;
using JewelryAuctionApplicationDAL.Models;
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
}
