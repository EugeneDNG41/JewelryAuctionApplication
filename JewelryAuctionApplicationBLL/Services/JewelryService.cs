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
    public IEnumerable<Jewelry> GetAll()
    {
        return _jewelryRepository.GetAll();
    }
}
