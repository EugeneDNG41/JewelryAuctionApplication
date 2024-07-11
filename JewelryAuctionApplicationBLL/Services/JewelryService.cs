using JewelryAuctionApplicationDAL.Models;
using JewelryAuctionApplicationDAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JewelryAuctionApplicationBLL.Services;

public class JewelryService : IJewelryService
{
    private readonly IJewelryRepository _jewelryRepository;
    public JewelryService(IJewelryRepository jewelryRepository)
    {
        _jewelryRepository = jewelryRepository;
    }
    public void Add(Jewelry jewelry)
    {
        _jewelryRepository.Add(jewelry);
    }
    public void Update(Jewelry jewelry)
    {
        _jewelryRepository.Update(jewelry);
    }
    public Jewelry? GetById(int id)
    {
        return _jewelryRepository.GetById(id);
    }
    public IEnumerable<Jewelry> GetAll()
    {
        return _jewelryRepository.GetAll();
    }

    public IEnumerable<Jewelry> GetByStatus(JewelryStatus status)
    {
        return _jewelryRepository.GetByStatus(status);
    }

    public IEnumerable<Jewelry> GetForAuction()
    {
        return _jewelryRepository.GetForAuction();
    }

    public IEnumerable<Jewelry> GetOnAuction()
    {
        return _jewelryRepository.GetOnAuction();
    }
    public IEnumerable<Jewelry> GetByEndedAuction()
    {
        return _jewelryRepository.GetByEndedAuction();
    }
}
