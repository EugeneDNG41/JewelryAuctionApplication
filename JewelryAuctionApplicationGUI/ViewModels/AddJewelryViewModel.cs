using JewelryAuctionApplicationBLL.Services;
using JewelryAuctionApplicationDAL.Models;
using JewelryAuctionApplicationGUI.Commands;
using JewelryAuctionApplicationGUI.Navigation;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace JewelryAuctionApplicationGUI.ViewModels;

public class AddJewelryViewModel : BaseViewModel
{
    private string jewelryName;
    public string JewelryName
    {
        get
        {
            return jewelryName;
        }

        set
        {
            jewelryName = value; //new value is inputted
            OnPropertyChanged(nameof(JewelryName));

            ClearErrors(nameof(JewelryName)); //clear previous error

            if (string.IsNullOrEmpty(JewelryName)) //check for error
            {
                AddError("Required", nameof(JewelryName));
            }
            OnErrorsChanged(nameof(JewelryName));
        }
    }
    private string description;
    public string Description
    {
        get
        {
            return description;
        }

        set
        {
            description = value;
            OnPropertyChanged(nameof(Description));

            ClearErrors(nameof(Description)); //clear previous error

            if (string.IsNullOrEmpty(Description)) //check for error
            {
                AddError("Required", nameof(Description));
            }
            OnErrorsChanged(nameof(Description));
        }
    }
    private string condition;
    public string Condition
    {
        get
        {
            return condition;
        }

        set
        {
            condition = value;
            OnPropertyChanged(nameof(Condition));
            ClearErrors(nameof(Condition)); //clear previous error

            if (string.IsNullOrEmpty(Condition)) //check for error
            {
                AddError("Required", nameof(Condition));
            }
            OnErrorsChanged(nameof(Condition));
        }
    }
    private decimal startingPrice;
    public decimal StartingPrice
    {
        get
        {
            return startingPrice;
        }

        set
        {
            startingPrice = value;
            OnPropertyChanged(nameof(StartingPrice));
            ClearErrors(nameof(StartingPrice)); //clear previous error

            if (StartingPrice <= 0) //check for error
            {
                AddError("Invalid Price", nameof(StartingPrice));
            }
            OnErrorsChanged(nameof(StartingPrice));
        }
    }
    public ObservableCollection<string> Categories =>
       new ObservableCollection<string>(GenerateCategoryList());
    private int category;
    public int Category
    {
        get
        {
            return category;
        }

        set
        {
            category = value;
            OnPropertyChanged(nameof(Category));
            ClearErrors(nameof(Category)); //clear previous error
        }
    }
    private BitmapImage image;
    public BitmapImage Image
    {
        get
        {
            return image;
        }

        set
        {
            image = value;
            OnPropertyChanged(nameof(Image));
            ClearErrors(nameof(Image)); //clear previous error
        }
    }
    public ICommand AddJewelryCommand { get; }
    public ICommand UploadImageCommand { get; }
    public ICommand CloseModalCommand { get; }
    private readonly Dictionary<string, List<string>> _propertyErrors = new();
    public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;
    public bool HasErrors => _propertyErrors.Any();
    public bool CanClick => !HasErrors;
    public AddJewelryViewModel(IJewelryService jewelryService,
        INavigationService closeModalNavigationService)
    {
        AddJewelryCommand = new AddJewelryCommand(this, jewelryService, closeModalNavigationService);
        UploadImageCommand = new UploadImageCommand(this);
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
    private List<string> GenerateCategoryList()
    {
        var categories = new List<string>();
        foreach (JewelryCategory category in Enum.GetValues(typeof(JewelryCategory)))
        {
            string categoryString = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(category.ToString().Replace("_", " and ").ToLower());
            categoryString = categoryString.Replace(" And ", " and ");
            categories.Add(categoryString);
        }
        return categories;
    }
}
