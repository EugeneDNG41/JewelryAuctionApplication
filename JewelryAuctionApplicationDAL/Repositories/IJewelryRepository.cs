using JewelryAuctionApplicationDAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JewelryAuctionApplicationDAL.Repositories;

public interface IJewelryRepository
{
    void Add(Jewelry jewelry);
    void Update(Jewelry jewelry);
    Jewelry? GetById(int id);
    IEnumerable<Jewelry> GetAll();
    IEnumerable<Jewelry> GetByStatus(JewelryStatus status);
    IEnumerable<Jewelry> GetForAuction();
    IEnumerable<Jewelry> GetOnAuction();
    IEnumerable<Jewelry> GetByEndedAuction();
}
