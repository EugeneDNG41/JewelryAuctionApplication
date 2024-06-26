﻿using JewelryAuctionApplicationGUI.Commands;
using JewelryAuctionApplicationBLL.Stores;
using System.Windows.Input;
using JewelryAuctionApplicationGUI.Navigation;

namespace JewelryAuctionApplicationGUI.ViewModels;

public class NavigationBarViewModel : BaseViewModel
{
    private readonly AccountStore _accountStore;

    public ICommand NavigateHomeCommand { get; }
    public ICommand NavigateLoginCommand { get; }
    public ICommand NavigateLogoutCommand { get; }
    public ICommand NavigateSignupCommand { get; }

    public bool IsLoggedIn => _accountStore.IsLoggedIn;
    public bool IsLoggedOut => !IsLoggedIn;

    public NavigationBarViewModel(AccountStore accountStore, 
        INavigationService homeNavigationService, 
        INavigationService loginNavigationService,
        INavigationService signupNavigationService)
    {
        _accountStore = accountStore;
        NavigateHomeCommand = new NavigateCommand(homeNavigationService);
        NavigateLoginCommand = new NavigateCommand(loginNavigationService);
        NavigateSignupCommand = new NavigateCommand(signupNavigationService);
        NavigateLogoutCommand = new LogoutCommand(_accountStore, homeNavigationService);

        _accountStore.CurrentAccountChanged += OnCurrentAccountChanged;
    }

    private void OnCurrentAccountChanged()
    {
        OnPropertyChanged(nameof(IsLoggedIn));
    }

    public override void Dispose()
    {
        _accountStore.CurrentAccountChanged -= OnCurrentAccountChanged;
        base.Dispose();
    }
}
