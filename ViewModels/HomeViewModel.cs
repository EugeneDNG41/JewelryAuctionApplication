using JewelryAuctionApplication.Commands;
using JewelryAuctionApplication.Services;
using System.Windows.Input;

namespace JewelryAuctionApplication.ViewModels;

public class HomeViewModel
{
    public static string WelcomeMessage => "Welcome to my application.";

    public ICommand NavigateLoginCommand { get; }

    public HomeViewModel(INavigationService loginNavigationService)
    {
        NavigateLoginCommand = new NavigateCommand(loginNavigationService);
    }
}
