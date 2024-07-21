using JewelryAuctionApplicationDAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JewelryAuctionApplicationBLL.Services;

public interface IAccountService
{
    Account? Authenticate(string username, string password);
    string Create(Account account);
    void Update(Account account);
    void ResetPassword(Account account);
    void Deactivate(int id);
    IEnumerable<Account> GetAll();
    Account? GetByUsername(string username);
    IEnumerable<Account> GetByRole(Role role);
    Account? GetById(int id);
    void ChangePassword(Account account, string newPassword);
}
