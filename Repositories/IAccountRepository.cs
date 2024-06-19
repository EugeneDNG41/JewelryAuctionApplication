using JewelryAuctionApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JewelryAuctionApplication.Repositories;

public interface IAccountRepository
{
    IEnumerable<Account> GetAll();
    Account? GetById(int id);
    Account? GetByEmail(string email);
    Account? GetByUsername(string username);
    Account? GetByPhoneNumber(string phone);
    void Add(Account acc);
    void Update(Account acc);
    void Delete(int id);
}
