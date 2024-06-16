using JewelryAuctionDemo;
using JewelryAuctionDemo.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JewelryAuctionSystem.Services;

public class AccountService : IAccountService
{
    public bool Authenticate(string username, string password)
    {
        throw new NotImplementedException();
    }

    public bool Register(Account account)
    {
        throw new NotImplementedException();
    }
}
