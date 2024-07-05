using JewelryAuctionApplicationDAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JewelryAuctionApplicationBLL.Services;

public interface IBidService
{
    void AddBid(Bid bid);
    IEnumerable<Bid> GetAll();
}
