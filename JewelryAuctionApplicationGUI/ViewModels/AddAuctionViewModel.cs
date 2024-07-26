using JewelryAuctionApplicationBLL.Services;
using JewelryAuctionApplicationDAL.Models;
using JewelryAuctionApplicationGUI.Commands;
using JewelryAuctionApplicationGUI.Navigation;
using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace JewelryAuctionApplicationGUI.ViewModels;

public class AddAuctionViewModel : BaseViewModel
{
    public Jewelry Jewelry { get; }
    private DateTime _endDate;
    public DateTime EndDate
    {
        get
        {
            return _endDate;
        }
        set
        {
            _endDate = value;
            OnPropertyChanged(nameof(EndDate));
            ClearErrors(nameof(EndDate)); //clear previous error

            if (EndDate < DateTime.Now) //check for error
            {
                AddError("End Date must be in the future", nameof(EndDate));
            }
            OnErrorsChanged(nameof(EndDate));
        }
    }
    public BitmapImage DisplayedImage =>  ByteArrayToBitmapImage(Jewelry.Image);

    public ICommand AddAuctionCommand { get; }
    public ICommand CloseModalCommand { get; }
    private readonly Dictionary<string, List<string>> _propertyErrors = new();
    public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;
    public bool HasErrors => _propertyErrors.Any();
    public bool CanClick => !HasErrors;
    public AddAuctionViewModel(Jewelry jewelry,
        IAuctionService auctionService, 
        IJewelryService jewelryService,
        INavigationService closeModalNavigationService,
        INavigationService returnJewelryManagementNavigationService)
    {
        Jewelry = jewelry;
        AddAuctionCommand = new AddAuctionCommand(this, auctionService, jewelryService, returnJewelryManagementNavigationService);
        CloseModalCommand = new CloseModalCommand(closeModalNavigationService);
        EndDate = DateTime.Now;
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
    public BitmapImage ByteArrayToBitmapImage(byte[] byteArray)
    {
        using (MemoryStream stream = new MemoryStream(byteArray))
        {
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            image.CacheOption = BitmapCacheOption.OnLoad;
            image.StreamSource = stream;
            image.EndInit();
            return image;
        }
    }
}
