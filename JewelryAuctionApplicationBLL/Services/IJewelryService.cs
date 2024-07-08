using JewelryAuctionApplicationDAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JewelryAuctionApplicationBLL.Services;

public interface IJewelryService
{
    void Add(Jewelry jewelry);
    void Update(Jewelry jewelry);
    IEnumerable<Jewelry> GetAll();
    IEnumerable<Jewelry> GetByStatus(JewelryStatus status);
    IEnumerable<Jewelry> GetForAuction();
    IEnumerable<Jewelry> GetOnAuction();
}
