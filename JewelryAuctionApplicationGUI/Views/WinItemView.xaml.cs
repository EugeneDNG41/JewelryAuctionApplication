using JewelryAuctionApplicationBLL.Services;
using JewelryAuctionApplicationBLL.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace JewelryAuctionApplicationGUI.Views
{
    /// <summary>
    /// Interaction logic for WinItemView.xaml
    /// </summary>
    public partial class WinItemView : UserControl
    {
        private readonly AuctionService _service;
        private readonly AccountStore accountStore;
        public WinItemView()
        {
            InitializeComponent(); HandleBeforeLoaded();
        }

        public void UpdateGridView()
        {
            listWonItem.ItemsSource = _service.GetWonAuction(accountStore.CurrentAccount.AccountId);
        }
        private void HandleBeforeLoaded()
        {
            UpdateGridView();
        }
    }
}
