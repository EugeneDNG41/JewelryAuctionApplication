using JewelryAuctionApplicationBLL.Services;
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
    private readonly IAccountService _service;
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
    public ICommand ChangePasswordCommand { get; }
    public ICommand CloseModalCommand { get; }
    private readonly Dictionary<string, List<string>> _propertyErrors = new();
    public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;
    public bool HasErrors => _propertyErrors.Any();
    public bool CanClick => !HasErrors;
    public ChangePasswordViewModel(IAccountService service,
        INavigationService closeModalNavigationService)
    {
        _service = service;
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
        OnPropertyChanged(nameof(CanClick));
    }

    public void ClearErrors(string propertyName)
    {
        if (_propertyErrors.Remove(propertyName))
        {
            OnErrorsChanged(propertyName); //make sure that the change should be notified accordingly
        }
    }
}
