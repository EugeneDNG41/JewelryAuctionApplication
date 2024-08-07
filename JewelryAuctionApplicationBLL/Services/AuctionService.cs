﻿using JewelryAuctionApplicationDAL.Models;
using JewelryAuctionApplicationDAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JewelryAuctionApplicationBLL.Services;

public class AuctionService : IAuctionService
{
    private readonly IAuctionRepository _repository;
    public AuctionService(IAuctionRepository repository)
    {
        _repository = repository;
    }
    public void Add(Auction auction)
    {
        _repository.Add(auction);
    }
    public void Update(Auction auction)
    {
        _repository.Update(auction);
    }
    public Auction? GetOngoingByJewelryId(int id)
    {
        return _repository.GetOngoingByJewelryId(id);
    }
    public Auction? GetById(int id)
    {
        return _repository.GetById(id);
    }
    public Auction? GetLatestByJewelryId(int id)
    {
        return _repository.GetLatestByJewelryId(id);
    }
    public async Task<IEnumerable<Auction>> GetAllLatestAsync()
    {
        return await _repository.GetAllLatestAsync();
    }
    public IEnumerable<Auction> GetWonAuction(int accountId)
    {
        return _repository.GetWonAuction(accountId);
    }
    public async Task UpdateAsync(Auction auction)
    {
        await _repository.UpdateAsync(auction);
    }
}
