using JewelryAuctionApplicationDAL.Context;
using JewelryAuctionApplicationDAL.Models;
using Microsoft.EntityFrameworkCore;

namespace JewelryAuctionApplicationDAL.Repositories;

public class AccountRepository : IAccountRepository
{
    private readonly JewelryAuctionContext _context;

    public AccountRepository(JewelryAuctionContext context)
    {
        _context = context;
    }

    public IEnumerable<Account> GetAll()
    {
        return _context.Accounts.ToList();
    }

    public Account? GetById(int id)
    {
        return _context.Accounts.FirstOrDefault(a => a.AccountId == id);
    }

    public Account? GetByEmail(string email)
    {
        return _context.Accounts.FirstOrDefault(a => a.Email == email);
    }

    public  Account? GetByUsername(string username)
    {
        return _context.Accounts.FirstOrDefault(a => a.Username == username);
    }

    public void Add(Account account)
    {
        _context.Accounts.Add(account);
        _context.SaveChanges();
    }

    public void Delete(int id)
    {
        var account = GetById(id);
        if (account != null)
        {
            _context.Accounts.Remove(account);
            _context.SaveChanges();
        }
    }

    public void Update(Account account)
    {
        var existingAccount = GetById(account.AccountId);
        if (existingAccount != null)
        {
            existingAccount.Email = account.Email;
            existingAccount.Username = account.Username;
            existingAccount.Email = account.Email;
            existingAccount.Role = account.Role;
            existingAccount.Status = account.Status;
            _context.Accounts.Update(existingAccount);
            _context.SaveChanges();
        }
    }

    public IEnumerable<Account> GetByRole(Role role)
    {
        return _context.Accounts.Where(a => a.Role == role);
    }
}
