using JewelryAuctionApplicationBLL.Services;
using JewelryAuctionApplicationDAL.Models;
using JewelryAuctionApplicationGUI.Navigation;
using JewelryAuctionApplicationGUI.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JewelryAuctionApplicationGUI.Commands;

public class CreateAccountCommand : BaseCommand
{
    private readonly CreateAccountViewModel _viewModel;
    private readonly IAccountService _accountService;
    private readonly INavigationService _navigationService;
    public CreateAccountCommand(CreateAccountViewModel viewModel, IAccountService accountService, INavigationService closeModalNavigationService)
    {
        _viewModel = viewModel;
        _accountService = accountService;
        _navigationService = closeModalNavigationService;
    }

    public override void Execute(object parameter)
    {
        if (string.IsNullOrEmpty(_viewModel.Username))
        {
            _viewModel.AddError("Required", nameof(_viewModel.Username));
            return;
        }
        if (string.IsNullOrEmpty(_viewModel.FullName))
        {
            _viewModel.AddError("Required", nameof(_viewModel.FullName));
            return;
        }
        if (string.IsNullOrEmpty(_viewModel.Email))
        {
            _viewModel.AddError("Required", nameof(_viewModel.Email));
            return;
        }
        var account = new Account
        {
            Username = _viewModel.Username,
            Email = _viewModel.Email,
            Password = "123",
            FullName = _viewModel.FullName,
            Role = (Role)(_viewModel.Role),
            Credit = 0,
            Status = true
        };
        string result = _accountService.Create(account);
        switch (result)
        {
            case "USERNAME_TAKEN":
                _viewModel.AddError("Username already taken", nameof(_viewModel.Username)); return;
            case "EMAIL_TAKEN":
                _viewModel.AddError("Email address already taken", nameof(_viewModel.Email)); return;
            case "SUCCESS":
                MessageBox.Show("Account created successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                _navigationService.Navigate();
                break;
        }
    }
}

