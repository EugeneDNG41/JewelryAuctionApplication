using JewelryAuctionApplicationBLL.Services;
using JewelryAuctionApplicationBLL.Stores;
using JewelryAuctionApplicationDAL.Models;
using JewelryAuctionApplicationGUI.Navigation;
using JewelryAuctionApplicationGUI.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JewelryAuctionApplicationGUI.Commands;

public class SignupCommand : BaseCommand
{
    private readonly SignupViewModel _viewModel;
    private readonly AccountStore _accountStore;
    private readonly INavigationService _navigationService;
    private readonly IAccountService _accountService;

    public SignupCommand(SignupViewModel viewModel, 
        AccountStore accountStore, 
        INavigationService navigationService, 
        IAccountService accountService)
    {
        _viewModel = viewModel;
        _accountStore = accountStore;
        _navigationService = navigationService;
        _accountService = accountService;
    }

    public override void Execute(object parameter)
    {
        if (string.IsNullOrEmpty(_viewModel.Username)) {
            _viewModel.AddError("Required", nameof(_viewModel.Username));
            return;
        }
        if (string.IsNullOrEmpty(_viewModel.FullName)) {
            _viewModel.AddError("Required", nameof(_viewModel.FullName));
            return;
        }
        if (string.IsNullOrEmpty(_viewModel.Email)) {
            _viewModel.AddError("Required", nameof(_viewModel.Email));
            return;
        }
        if (string.IsNullOrEmpty(_viewModel.Password)) {
            _viewModel.AddError("Required", nameof(_viewModel.Password));
            return;
        }
        var account = new Account
        {
            Username = _viewModel.Username,
            Email = _viewModel.Email,
            Password = _viewModel.Password,
            FullName = _viewModel.FullName,
            Role = Role.USER,
            Status = true
        };
        string result = _accountService.Register(account);
        switch (result)
        {
            case "USERNAME_TAKEN":
                _viewModel.AddError("Username already taken", nameof(_viewModel.Username)); return;
            case "EMAIL_TAKEN":
                _viewModel.AddError("Email address already taken", nameof(_viewModel.Email)); return;           
            case "SUCCESS":
                _navigationService.Navigate();
                break;                
        }
    }
}
