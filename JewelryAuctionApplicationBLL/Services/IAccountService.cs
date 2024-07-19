using JewelryAuctionApplicationDAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JewelryAuctionApplicationBLL.Services;

public interface IAccountService
{
    Task<Account?> Authenticate(string username, string password);
    string Create(Account account);
    void Update(Account account);
    void ResetPassword(Account account);
    void Deactivate(int id);
    IEnumerable<Account> GetAll();
    Task<Account?> GetByUsername(string username);
    IEnumerable<Account> GetByRole(Role role);
    Account? GetById(int id);
    void CreateAdmin();
    void ChangePassword(Account account, string newPassword);
    Task UpdateAsync(Account account);
}
