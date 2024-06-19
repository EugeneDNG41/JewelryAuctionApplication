using JewelryAuctionApplication.Models;
using JewelryAuctionApplication.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JewelryAuctionApplication.Services;
using System.Windows.Forms;

namespace JewelryAuctionApplication.Services;

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
            return Account;
        } else
        {
            return null;
        }
    }
    public void Register(Account account)
    {
        var existingEmail = _accountRepository.GetByEmail(account.Email);
        var existingUsername = _accountRepository.GetByUsername(account.Username);
        var existingPhoneNumber = _accountRepository.GetByPhoneNumber(account.PhoneNumber);
        var hashPassword = _passwordHasher.Hash(account.Password);
        var newAccount = new Account
        {
            Username = account.Username,
            Password = hashPassword,
            PhoneNumber = account.PhoneNumber,
            FullName = account.FullName,
            Role = Role.USER,
            Status = true,
            Image = account.Image ?? string.Empty
        };
        _accountRepository.Add(newAccount);
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
