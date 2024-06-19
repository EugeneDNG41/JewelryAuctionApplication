using JewelryAuctionApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JewelryAuctionApplication.Services;

public interface IAccountService
{
    Account? Authenticate(string username, string password);
    void Register(Account account);
    bool Update(Account account);
    bool Deactivate(int id);
    List<Account> GetAccounts();
    List<Account> GetByUsername(string username);
    List<Account> GetByRole(Role role);

}
