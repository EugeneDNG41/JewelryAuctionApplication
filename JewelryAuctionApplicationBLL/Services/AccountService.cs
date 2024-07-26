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
        var account = _accountRepository.GetByUsername(username);
        if (account != null && _passwordHasher.Verify(account.Password, password))
        {
            return account;
        }
        return null;
    }
    public void CreateAdmin()
    {
        var existingAdmin = _accountRepository.GetByRole(Role.ADMIN).FirstOrDefault();
        if (existingAdmin == null)
        {
            var admin = new Account
            {
                Username = "admin",
                Password = _passwordHasher.Hash("admin"),
                Email = "admin@admin.com",
                FullName = "Admin",
                Credit = 0,
                Role = Role.ADMIN,
                Status = true
            };
            _accountRepository.Add(admin);
        } else if (!existingAdmin.Status)
        {
            existingAdmin.Status = true;
            Update(existingAdmin);
        }
    }
    public string Create(Account account)
    {
        var existingEmail =  GetByEmail(account.Email);
        if (existingEmail != null)
        {
            return "EMAIL_TAKEN";
        }

        var existingUsername = GetByUsername(account.Username);
        if (existingUsername != null)
        {
            return "USERNAME_TAKEN";
        }

        var hashPassword = _passwordHasher.Hash(account.Password);
        account.Password = hashPassword;
        _accountRepository.Add(account);
        return "SUCCESS";
    }

    public void Deactivate(int id)
    {
        var account = _accountRepository.GetById(id);
        if (account != null)
        {
            account.Status = false;
            _accountRepository.Update(account);
        }
    }

    public IEnumerable<Account> GetAll()
    {
        return _accountRepository.GetAll();
    }

    public IEnumerable<Account> GetByRole(Role role)
    {
        return _accountRepository.GetByRole(role);
    }

    public Account? GetByUsername(string username)
    {
        return _accountRepository.GetByUsername(username);
    }

    public void Update(Account account)
    {
        _accountRepository.Update(account);
    }
    public void ResetPassword(Account account)
    {
        var hashPassword = _passwordHasher.Hash("123");
        account.Password = hashPassword;
        _accountRepository.Update(account);
    }
    public Account? GetById(int id)
    {
        return _accountRepository.GetById(id);
    }

    public void ChangePassword(Account account, string newPassword)
    {
        var hashPassword = _passwordHasher.Hash(newPassword);
        account.Password = hashPassword;
        _accountRepository.Update(account);
    }
    public Account? GetByEmail(string email)
    {
        return _accountRepository.GetByEmail(email);
    }
    public async Task UpdateAsync(Account account)
    {
        await _accountRepository.UpdateAsync(account);
    }
}
