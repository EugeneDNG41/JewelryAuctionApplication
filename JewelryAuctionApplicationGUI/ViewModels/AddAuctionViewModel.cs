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
    private decimal _currentPrice;
    public decimal CurrentPrice
    {
        get
        {
            return _currentPrice;
        }
        set
        {
            _currentPrice = value;
            OnPropertyChanged(nameof(CurrentPrice));           
            ClearErrors(nameof(CurrentPrice)); //clear previous error

            if (CurrentPrice < 0) //check for error
            {
                AddError("Current Price must be greater than 0", nameof(CurrentPrice));
            }
            OnErrorsChanged(nameof(CurrentPrice));
        }
    }
    private ObservableCollection<Jewelry> _jewelries;
    public ObservableCollection<Jewelry> Jewelries
    {
        get
        {
            return _jewelries;
        }
        set
        {
            _jewelries = value;
            OnPropertyChanged(nameof(Jewelries));
        }
    }
    private Jewelry _jewelry;
    public Jewelry Jewelry
    {
        get
        {
            return _jewelry;
        }
        set
        {
            _jewelry = value;
            OnPropertyChanged(nameof(Jewelry));
            OnPropertyChanged(nameof(DisplayedImage));
            ClearErrors(nameof(Jewelry)); //clear previous error

            if (Jewelry == null) //check for error
            {
                AddError("Jewelry is required", nameof(Jewelry));
            }
            else
            {
                CurrentPrice = Jewelry.StartingPrice; // Set CurrentPrice to StartingPrice of selected Jewelry
            }
            OnErrorsChanged(nameof(Jewelry));
        }
    }
    public BitmapImage? DisplayedImage => Jewelry != null ? ByteArrayToBitmapImage(Jewelry.Image) : null;

    public ICommand AddAuctionCommand { get; }
    public ICommand CloseModalCommand { get; }
    public AddAuctionViewModel(IAuctionService auctionService, 
        IJewelryService jewelryService,
        INavigationService closeModalNavigationService)
    {
        Jewelries = new ObservableCollection<Jewelry>(jewelryService.GetAll());
        AddAuctionCommand = new AddAuctionCommand(this, auctionService, jewelryService);
        CloseModalCommand = new CloseModalCommand(closeModalNavigationService);
        EndDate = DateTime.Now;
    }
    private readonly Dictionary<string, List<string>> _propertyErrors = new();
    public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;
    public bool HasErrors => _propertyErrors.Any();
    public bool CanClick => !HasErrors;
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
