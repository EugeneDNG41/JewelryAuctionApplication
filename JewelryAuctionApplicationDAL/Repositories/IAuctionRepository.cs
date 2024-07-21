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
    void Update(Auction auction);
    IEnumerable<Auction> GetAllLatest();
    IEnumerable<Auction> GetByJewelryId(int jewelryId);
    Auction? GetOngoingByJewelryId(int id);
    Auction? GetById(int id);
    Auction? GetLatestByJewelryId(int jewelryId);
    IEnumerable<Auction> GetWonAuction(int accountId);
}
