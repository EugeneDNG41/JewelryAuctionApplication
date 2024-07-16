using JewelryAuctionApplicationBLL.Services;
using JewelryAuctionApplicationDAL.Models;
using JewelryAuctionApplicationGUI.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace JewelryAuctionApplicationGUI.ViewModels;

public class ViewDetailsViewModel : BaseViewModel
{
    public JewelryManagerViewModel JewelryDetails {  get; private set; }
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
            OnPropertyChanged(nameof(CanClick));
        }
    }
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
            OnPropertyChanged(nameof(CanClick));
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
            OnPropertyChanged(nameof(CanClick));

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
            OnPropertyChanged(nameof(CanClick));
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
            OnPropertyChanged(nameof(CanClick));
        }
    }
    public ICommand UploadUpdateImageCommand { get; }
    public ICommand UpdateJewelryCommand { get; }
    public bool CanClick => CheckCanclick();


    public ViewDetailsViewModel(JewelryManagerViewModel jewelryManagerViewModel, IJewelryService jewelryService)
    {
        
        GetOriginalInformation(jewelryManagerViewModel);
        UploadUpdateImageCommand = new UploadUpdateImageCommand(this);
    }
    private void GetOriginalInformation(JewelryManagerViewModel jewelryManagerViewModel)
    {
        JewelryDetails = jewelryManagerViewModel;
        JewelryName = jewelryManagerViewModel.Jewelry.JewelryName;
        Description = jewelryManagerViewModel.Jewelry.Description;
        Condition= jewelryManagerViewModel.Jewelry.Condition;
        StartingPrice= jewelryManagerViewModel.Jewelry.StartingPrice;
        int i = (int)jewelryManagerViewModel.Jewelry.JewelryCategory;
        Category = i;
        Image = ByteArrayToBitmapImage(JewelryDetails.Jewelry.Image);
    }

    private BitmapImage ByteArrayToBitmapImage(byte[] byteArray)
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
    private bool CheckCanclick()
    {
        if (JewelryName != JewelryDetails.Jewelry.JewelryName
            || Description != JewelryDetails.Jewelry.Description
            || Condition != JewelryDetails.Jewelry.Condition
            || StartingPrice != JewelryDetails.Jewelry.StartingPrice
            || Category != (int)JewelryDetails.Jewelry.JewelryCategory
            || Image != ByteArrayToBitmapImage(JewelryDetails.Jewelry.Image))
        {
            return true;
        } else { return false; }
    }
}
