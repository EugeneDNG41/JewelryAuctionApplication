using JewelryAuctionApplicationDAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JewelryAuctionApplicationDAL.Repositories;

public interface IAuctionRepository
{
    void Add(Auction auction);
    IEnumerable<Auction> GetByJewelryId(int jewelryId);
    Auction? GetOngoingByJewelryId(int id);
    Auction? GetById(int id);
}
