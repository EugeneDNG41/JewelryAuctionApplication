using JewelryAuctionApplicationBLL.Services;
using JewelryAuctionApplicationDAL.Models;
using JewelryAuctionApplicationGUI.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JewelryAuctionApplicationGUI.Commands;

public class UpdateProfileCommand : BaseCommand
{
    private readonly IAccountService _accountService;
    private readonly ProfileViewModel _viewModel;
    public UpdateProfileCommand(IAccountService accountService,
        ProfileViewModel profileViewModel)
    {
        _accountService = accountService;
        _viewModel = profileViewModel;
    }
    public override void Execute(object parameter)
    {
        if (string.IsNullOrEmpty(_viewModel.Username))
        {
            _viewModel.AddError("Required", nameof(_viewModel.Username));
            return;
        }
        if (_accountService.GetByUsername(_viewModel.Username) != null && _viewModel.Username != _viewModel.Account.Username)
        {
            _viewModel.AddError("This username has been existed", nameof(_viewModel.Username));
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
            AccountId = _viewModel.Account.AccountId,
            Username = _viewModel.Username,
            FullName = _viewModel.FullName,
            Email = _viewModel.Email,
            Password = _viewModel.Account.Password,
            Status = _viewModel.Account.Status,
            Role = _viewModel.Account.Role,
        };
        _accountService.Update(account);
    }
}
