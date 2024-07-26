using JewelryAuctionApplicationBLL.Services;
using JewelryAuctionApplicationBLL.Stores;
using JewelryAuctionApplicationGUI.Commands;
using JewelryAuctionApplicationGUI.Navigation;
using Microsoft.Identity.Client;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace JewelryAuctionApplicationGUI.ViewModels;

public class ChangePasswordViewModel : BaseViewModel
{
    private string oldPassword;
    private string newPassword;
    private string confirmPassword;

    public string OldPassword
    {
        get => oldPassword;
        set
        {
            oldPassword = value; //new value is inputted
            OnPropertyChanged(nameof(OldPassword));

            ClearErrors(nameof(OldPassword)); //clear previous error

            if (string.IsNullOrEmpty(OldPassword)) //check for error
            {
                AddError("Required", nameof(OldPassword));
            }
            OnErrorsChanged(nameof(OldPassword));
        }
    }
    public string NewPassword
    {
        get => newPassword;
        set
        {
            newPassword = value; //new value is inputted
            OnPropertyChanged(nameof(NewPassword));

            ClearErrors(nameof(NewPassword)); //clear previous error

            if (string.IsNullOrEmpty(NewPassword)) //check for error
            {
                AddError("Required", nameof(NewPassword));
            }
            OnErrorsChanged(nameof(NewPassword));
        }
    }
    public string ConfirmPassword
    {
        get => confirmPassword;
        set
        {
            confirmPassword = value; //new value is inputted
            OnPropertyChanged(nameof(ConfirmPassword));

            ClearErrors(nameof(ConfirmPassword)); //clear previous error

            if (string.IsNullOrEmpty(ConfirmPassword)) //check for error
            {
                AddError("Required", nameof(ConfirmPassword));
            }
            OnErrorsChanged(nameof(ConfirmPassword));
        }
    }
    private string _errorMessage;
    public string ErrorMessage
    {
        get
        {
            return _errorMessage;
        }

        set
        {
            _errorMessage = value;
            OnPropertyChanged(nameof(ErrorMessage));
        }
    }
    public ICommand ChangePasswordCommand { get; }
    public ICommand CloseModalCommand { get; }
    private readonly Dictionary<string, List<string>> _propertyErrors = new();
    public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;
    public ChangePasswordViewModel(IAccountService service,
        AccountStore accountStore,
        INavigationService closeModalNavigationService)
    {
        ChangePasswordCommand = new ChangePasswordCommand(this, accountStore, closeModalNavigationService,service);
        CloseModalCommand = new CloseModalCommand(closeModalNavigationService);
    }

    public IEnumerable GetErrors(string propertyName)
    {
        return _propertyErrors.GetValueOrDefault(propertyName, null);
    }
    public void AddError(string errorMessage, string propertyName)
    {
        if (!_propertyErrors.ContainsKey(propertyName))
        {
            _propertyErrors.Add(propertyName, new List<string>());
        }
        _propertyErrors[propertyName].Add(errorMessage);
        OnErrorsChanged(propertyName);
    }

    private void OnErrorsChanged(string propertyName)
    {
        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
    }

    public void ClearErrors(string propertyName)
    {
        if (_propertyErrors.Remove(propertyName))
        {
            OnErrorsChanged(propertyName); //make sure that the change should be notified accordingly
        }
    }
}
