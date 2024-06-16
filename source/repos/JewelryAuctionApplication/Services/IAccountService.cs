using JewelryAuctionDemo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JewelryAuctionSystem.Services;

public interface IAccountService
{
    bool Authenticate(string username, string password);
    bool Register(Account account);
}
