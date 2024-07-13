using JewelryAuctionApplicationBLL.Services;
using JewelryAuctionApplicationBLL.Stores;
using JewelryAuctionApplicationGUI.Navigation;
using JewelryAuctionApplicationGUI.ViewModels;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JewelryAuctionApplicationGUI.Commands;

public class AddCreditCommand : BaseCommand
{
    private readonly AddCreditViewModel _viewModel;
    private readonly AccountStore _accountStore;
    private readonly INavigationService _navigationService;
    private readonly IAccountService _accountService;
    public AddCreditCommand(AddCreditViewModel addCreditViewModel, 
        AccountStore accountStore, 
        INavigationService closeModalNavigationService, 
        IAccountService accountService)
    {
        _viewModel = addCreditViewModel;
        _accountStore = accountStore;
        _navigationService = closeModalNavigationService;
        _accountService = accountService;
    }
    public override  void Execute(object parameter)
    {
        if (_viewModel.CreditAmount <= 0)
        {
            MessageBox.Show("Credit amount must be greater than 0");
            //_viewModel.AddError("Credit amount must be greater than 0", nameof(_viewModel.CreditAmount));
            return;
        }
        if (_viewModel.account != null)
        {
            _viewModel.account.Credit += _viewModel.CreditAmount;
            _accountService.Update(_viewModel.account);
            _viewModel.Refresh();
            MessageBox.Show("Credit added successfully");

            //_navigationService.Navigate();
        } else
        {
            MessageBox.Show("You must be logged in to add credit");
        }
        
    }
}
