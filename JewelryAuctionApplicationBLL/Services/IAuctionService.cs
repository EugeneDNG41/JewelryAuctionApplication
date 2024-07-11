using JewelryAuctionApplicationDAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JewelryAuctionApplicationBLL.Services;

public interface IAuctionService
{
    void Add(Auction auction);
    void Update(Auction auction);
    Auction? GetOngoingByJewelryId(int id);
    Auction? GetById(int id);
    Auction? GetLatestByJewelryId(int id);
    IEnumerable<Auction> GetAllLatest();
}
