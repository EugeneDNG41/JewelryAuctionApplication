﻿using JewelryAuctionApplicationDAL.Models;
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

    public string Create(Account account)
    {
        var existingEmail =  _accountRepository.GetByEmail(account.Email);
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
        var thisAccount = _accountRepository.GetById(account.AccountId);
        if (thisAccount != null && !_passwordHasher.Verify(thisAccount.Password, account.Password))
        {
            var hashPassword = _passwordHasher.Hash(account.Password);
            account.Password = hashPassword;
        }
        _accountRepository.Update(account);
    }
    public Account? GetById(int id)
    {
        return _accountRepository.GetById(id);
    }
}
