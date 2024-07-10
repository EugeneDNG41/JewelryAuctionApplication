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
    public IEnumerable<Auction> GetByJewelryId(int jewelryId)
    {
        return _context.Auctions.Where(a => a.JewelryId == jewelryId);
    }
    public Auction? GetOngoingByJewelryId(int id)
    {
        return _context.Auctions.Include(a => a.Bids).FirstOrDefault(a => a.JewelryId == id && a.EndDate > DateTime.Now);
    }
    public Auction? GetById(int id) => _context.Auctions.Include(a => a.Bids).FirstOrDefault(a => a.AuctionId == id);
}
