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

public class JewelryRepository : IJewelryRepository
{
    private readonly JewelryAuctionContext _context;
    
    public JewelryRepository(JewelryAuctionContext context)
    {
        _context = context;
    }
    public void Add(Jewelry jewelry)
    {
        _context.Jewelries.Add(jewelry);
        _context.SaveChanges();
    }
    public void Update(Jewelry jewelry)
    {
        _context.Jewelries.Update(jewelry);
        _context.SaveChanges();
    }
    public Jewelry? GetById(int id) => _context.Jewelries.FirstOrDefault(j => j.JewelryId == id);
    public IEnumerable<Jewelry> GetAll() => _context.Jewelries.Include(j => j.Auctions).ThenInclude(a => a.Bids);
    public IEnumerable<Jewelry> GetByStatus(JewelryStatus status) => _context.Jewelries.Include(j => j.Auctions).ThenInclude(a => a.Bids).Where(j => j.Status == status);
    public IEnumerable<Jewelry> GetForAuction()
    {
        var jewelries = GetByStatus(JewelryStatus.READY);
        return jewelries.Where(j => j.Auctions.IsNullOrEmpty() || j.Auctions.All(a => a.EndDate < DateTime.Now));
    }
    public IEnumerable<(Jewelry Jewelry, Auction LatestAuction)> GetJewelriesWithOngoingAuctions()
    {
        var jewelries = GetOnAuction()
            .Select(j => new
            {
                Jewelry = j,
                LatestAuction = j.Auctions.OrderByDescending(a => a.AuctionId).First()
            });

        return jewelries.Select(j => (j.Jewelry, j.LatestAuction));
    }
    public IEnumerable<(Jewelry Jewelry, Auction LatestAuction)> GetJewelriesWithEndedAuctions()
    {
        var jewelries = GetByEndedAuction()
            .Select(j => new
            {
                Jewelry = j,
                LatestAuction = j.Auctions.OrderByDescending(a => a.AuctionId).First()
            });

        return jewelries.Select(j => (j.Jewelry, j.LatestAuction));
    }
    public IEnumerable<Jewelry> GetOnAuction()
    {
        var jewelries = GetByStatus(JewelryStatus.ACTIVE);
        return jewelries.Where(j => !j.Auctions.IsNullOrEmpty() && j.Auctions.Any(a => a.EndDate > DateTime.Now));
    }
    public IEnumerable<Jewelry> GetByEndedAuction()
    {
        var jewelries = GetAll();
        return jewelries.Where(j => j.Status != JewelryStatus.DELETED && !j.Auctions.IsNullOrEmpty() && j.Auctions.All(a => a.EndDate < DateTime.Now));
    }
}
