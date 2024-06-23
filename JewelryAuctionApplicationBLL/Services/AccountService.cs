using JewelryAuctionApplicationDAL.Models;
using JewelryAuctionApplicationDAL.Repositories;

namespace JewelryAuctionApplicationBLL.Services;

public class AccountService : IAccountService
{
    private readonly IAccountRepository _accountRepository;
    private readonly IPasswordHasher _passwordHasher;
    public AccountService(IAccountRepository accountRepository, IPasswordHasher passwordHasher)
    {
        _accountRepository = accountRepository;
        _passwordHasher = passwordHasher;
    }
    public Account? Authenticate(string username, string password)
    {
        var Account = _accountRepository.GetByUsername(username);
        if (Account != null)
        {
            var passCheck = _passwordHasher.Verify(Account.Password, password);
            if (passCheck) { return Account; }
            return Account;
        } else
        {
            return null;
        }
    }
    public string Register(Account account)
    {
        var existingEmail = _accountRepository.GetByEmail(account.Email);
        if (existingEmail != null)
        {
            return "EMAIL_TAKEN";
        }
        var existingUsername = _accountRepository.GetByUsername(account.Username);
        if (existingUsername != null)
        {
            return "USERNAME_TAKEN";
        }
        var hashPassword = _passwordHasher.Hash(account.Password);
        var newAccount = new Account
        {
            Username = account.Username,
            Email = account.Email,
            Password = hashPassword,
            //PhoneNumber = account.PhoneNumber,
            FullName = account.FullName,
            Role = Role.USER,
            Status = true
        };
        _accountRepository.Add(newAccount);
        return "SUCCESS";
    }

    public bool Deactivate(int id)
    {
        var account = _accountRepository.GetById(id);
        if (account != null)
        {
            account.Status = false;
            _accountRepository.Update(account);
            return true;
        }
        else
        {
            return false;
        }
    }

    public List<Account> GetAccounts()
    {
        throw new NotImplementedException();
    }

    public List<Account> GetByRole(Role role)
    {
        throw new NotImplementedException();
    }

    public List<Account> GetByUsername(string username)
    {
        throw new NotImplementedException();
    }

    

    public bool Update(Account account)
    {
        throw new NotImplementedException();
    }
}
