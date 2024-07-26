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
    public void Update(Auction auction)
    {
        _context.Auctions.Update(auction);
        _context.SaveChanges();
    }
    public async Task UpdateAsync(Auction auction)
    {
        _context.Auctions.Update(auction);
        await _context.SaveChangesAsync();
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
    public Auction? GetLatestByJewelryId(int jewelryId)
    {
        return _context.Auctions.Include(a => a.Bids).ThenInclude(b => b.Account).Where(a => a.JewelryId == jewelryId).OrderByDescending(a => a.AuctionId).FirstOrDefault();
    }
    public async Task<IEnumerable<Auction>> GetAllLatestAsync()
    {
        var latestAuctions = await _context.Auctions.Include(a => a.Jewelry).Include(a => a.Bids).ThenInclude(b => b.Account)
                .Where(a => a.Jewelry.Status == JewelryStatus.ACTIVE)
                .GroupBy(a => a.JewelryId)
                .Select(g => g.OrderByDescending(a => a.AuctionId).First()).ToListAsync();
        return latestAuctions;
    }
    public IEnumerable<Auction> GetWonAuction(int accountId)
    {
        return _context.Auctions.Include(a => a.AccountId == accountId);
    }
}
