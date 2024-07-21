using JewelryAuctionApplicationBLL.Services;
using JewelryAuctionApplicationGUI.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JewelryAuctionApplicationGUI.Commands;

public class DeleteProfileCommand : BaseCommand
{
    private readonly IAccountService _accountService;
    private readonly ProfileViewModel _profileViewModel;

    public DeleteProfileCommand(IAccountService accountService, ProfileViewModel profileViewModel)
    {
        _accountService = accountService;
        _profileViewModel = profileViewModel;
    }

    public override void Execute(object parameter)
    {
        _profileViewModel.Account.Status = false;
        _accountService.Update(_profileViewModel.Account);
    }
}
