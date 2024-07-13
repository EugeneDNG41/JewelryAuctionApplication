using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TextBox = System.Windows.Controls.TextBox;

namespace JewelryAuctionApplicationGUI.Views
{
    /// <summary>
    /// Interaction logic for AddAuctionView.xaml
    /// </summary>
    public partial class AddAuctionView : UserControl
    {
        public AddAuctionView()
        {
            InitializeComponent();
        }
        private void OnPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !double.TryParse(e.Text, out _);
        }
        private void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox? textBox = sender as TextBox;
            if (string.IsNullOrEmpty(textBox?.Text))
            {
                textBox.Text = "0";
                textBox.CaretIndex = 1; // Move caret to the end
            }
        }
    }
}
