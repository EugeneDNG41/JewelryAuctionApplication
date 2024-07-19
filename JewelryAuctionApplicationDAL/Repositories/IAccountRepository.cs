using JewelryAuctionApplicationDAL.Models;

namespace JewelryAuctionApplicationDAL.Repositories;


public interface IAccountRepository
{
    Task<Account?> GetByUsername(string username);
    Account? GetByEmail(string email);
    Account? GetById(int id);
    void Add(Account account);
    void Update(Account account);
    IEnumerable<Account> GetAll();
    IEnumerable<Account> GetByRole(Role role);
    Task UpdateAsync(Account account);
}