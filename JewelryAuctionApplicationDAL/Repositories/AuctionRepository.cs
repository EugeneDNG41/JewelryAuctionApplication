using JewelryAuctionApplicationDAL.Context;
using JewelryAuctionApplicationDAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JewelryAuctionApplicationDAL.Repositories;

public class AuctionRepository : IAuctionRepository
{
    private readonly JewelryAuctionContext _context;
    public AuctionRepository(JewelryAuctionContext context)
    {
        _context = context;
    }
    public void Add(Auction auction)
    {
        _context.Auctions.Add(auction);
        _context.SaveChanges();
    }
}
