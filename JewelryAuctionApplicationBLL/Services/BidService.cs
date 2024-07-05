﻿using JewelryAuctionApplicationDAL.Models;
using JewelryAuctionApplicationDAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JewelryAuctionApplicationBLL.Services;

public class BidService : IBidService
{
    private readonly IBidRepository _bidRepository;
    public BidService(IBidRepository bidRepository)
    {
        _bidRepository = bidRepository;
    }
    public void AddBid(Bid bid)
    {
        _bidRepository.AddBid(bid);
    }
    public IEnumerable<Bid> GetAll()
    {
        return _bidRepository.GetAll();
    }
}
