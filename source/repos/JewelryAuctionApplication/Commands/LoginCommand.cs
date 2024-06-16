using JewelryAuctionDemo.Models;
using JewelryAuctionSystem.Services;
using JewelryAuctionSystem.Stores;
using JewelryAuctionSystem.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JewelryAuctionSystem.Commands;

public class LoginCommand : BaseCommand
{
    private readonly LoginViewModel _viewModel;
    private readonly AccountStore _accountStore;
    private readonly INavigationService _navigationService;
    private readonly IAccountService _accountService;

    public LoginCommand(LoginViewModel viewModel, AccountStore accountStore, INavigationService navigationService)
    {
        _viewModel = viewModel;
        _accountStore = accountStore;
        _navigationService = navigationService;
        _accountService = new AccountService();
    }

    public override void Execute(object parameter)
    {
        Account account = new Account()
        {
            Email = _viewModel.Username,
            Username = _viewModel.Username
        };

        _accountStore.CurrentAccount = account;

        _navigationService.Navigate();
    }
}
