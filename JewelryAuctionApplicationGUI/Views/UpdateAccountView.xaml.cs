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
    /// Interaction logic for UpdateAccountView.xaml
    /// </summary>
    public partial class UpdateAccountView : UserControl
    {
        public UpdateAccountView()
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
