using JewelryAuctionSystem.ViewModels;
using JewelryAuctionSystem.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace JewelryAuctionDemo.ViewModels;

public class MainWindowViewModel : BaseViewModel
{
    public ICommand ShowLoginCommand { get; }
    //public ICommand ShowLogoutCommand { get; }
    public MainWindowViewModel() 
    {
        ShowLoginCommand = new RelayCommand(ShowLogin);
        //ShowLogoutCommand = new RelayCommand(ShowLogout);
    }

    private void ShowLogin(object obj)
    {
        var loginView = new LoginView
        {
            DataContext = new LoginViewModel()
        };
        // Create a new Window to host the LoginView
        var loginWindow = new Window
        {
            Content = loginView,
            Title = "Login", // Optional: Set a title for the login window
                            // Optionally configure the login window's size and behavior
            Width = 400,
            Height = 500,
            //ResizeMode = ResizeMode.CanResizeWithGrip // Allow resizing
        };

        // Show the login window as a modal dialog
        loginWindow.ShowDialog();

    }
}