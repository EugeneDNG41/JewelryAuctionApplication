    using JewelryAuctionApplicationDAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JewelryAuctionApplicationDAL.Repositories;

public interface IBidRepository
{
    void AddBid(Bid bid);
    IEnumerable<Bid> GetAll();
    Bid? GetHighestBid(int auctionId);
    IEnumerable<Bid> GetByAuctionId(int id);
    decimal GetBidBalanceByAccountId(int accountId);
    Task<decimal> GetCulmulativeBidAmountByAccountIdAsync(int id);
}
