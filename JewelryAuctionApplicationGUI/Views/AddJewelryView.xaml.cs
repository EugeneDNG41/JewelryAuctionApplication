using System.Windows.Controls;
using System.Windows.Input;


namespace JewelryAuctionApplicationGUI.Views
{
    /// <summary>
    /// Interaction logic for AddJewelryView.xaml
    /// </summary>
    public partial class AddJewelryView : UserControl
    {
        public AddJewelryView()
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
