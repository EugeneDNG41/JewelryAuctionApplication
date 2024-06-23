using JewelryAuctionApplicationDAL.Models;

namespace JewelryAuctionApplicationDAL.Repositories;

public interface IAccountRepository
{
    IEnumerable<Account> GetAll();
    Account? GetById(int id);
    Account? GetByEmail(string email);
    Account? GetByUsername(string username);
    //Account? GetByPhoneNumber(string phone);
    void Add(Account acc);
    void Update(Account acc);
    void Delete(int id);
}
