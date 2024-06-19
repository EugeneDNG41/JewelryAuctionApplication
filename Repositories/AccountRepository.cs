using JewelryAuctionApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace JewelryAuctionApplication.Repositories;

public class AccountRepository : IAccountRepository
{
    private readonly JewelryAuctionContext _context;
    public AccountRepository()
    {
        _context = new JewelryAuctionContext();
    }
    public IEnumerable<Account> GetAll() => _context.Accounts;
    
    public Account? GetById(int id) => _context.Accounts.FirstOrDefault(a => a.AccountId == id);
    public Account? GetByEmail(string email) => _context.Accounts.FirstOrDefault(a => a.Email == email);
    public Account? GetByPhoneNumber(string phone) => _context.Accounts.FirstOrDefault(a => a.PhoneNumber == phone);
    public Account? GetByUsername(string username) => _context.Accounts.FirstOrDefault(a => a.Username == username);
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
            _context.Update(account);
            _context.SaveChanges();
        }
    }
}
