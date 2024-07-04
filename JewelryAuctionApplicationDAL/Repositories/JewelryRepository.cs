using JewelryAuctionApplicationDAL.Context;
using JewelryAuctionApplicationDAL.Models;
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
    public IEnumerable<Jewelry> GetAll() => _context.Jewelries;
}
