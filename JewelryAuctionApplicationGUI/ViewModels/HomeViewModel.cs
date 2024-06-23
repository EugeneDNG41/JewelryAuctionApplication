
using JewelryAuctionApplicationGUI.Commands;
using JewelryAuctionApplicationGUI.Navigation;
using System.Windows.Input;

namespace JewelryAuctionApplicationGUI.ViewModels;

public class HomeViewModel : BaseViewModel
{
    public static string WelcomeMessage => "Welcome to my application.";

    public ICommand NavigateLoginCommand { get; }

    public HomeViewModel(INavigationService loginNavigationService)
    {
        NavigateLoginCommand = new NavigateCommand(loginNavigationService);
    }
}
