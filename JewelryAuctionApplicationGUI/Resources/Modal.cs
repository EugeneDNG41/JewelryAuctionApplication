
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;


namespace JewelryAuctionApplicationGUI.Resources;
public class Modal : ContentControl
{
    public static readonly DependencyProperty IsOpenProperty =
        DependencyProperty.Register("IsOpen", typeof(bool), typeof(Modal),
            new PropertyMetadata(false)); //default false

    public bool IsOpen
    {
        get { return (bool)GetValue(IsOpenProperty); }
        set { SetValue(IsOpenProperty, value); }
    }

    static Modal()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(Modal), new FrameworkPropertyMetadata(typeof(Modal)));
        BackgroundProperty.OverrideMetadata(typeof(Modal), new FrameworkPropertyMetadata(CreateDefaultBackground()));
    }

    private static object CreateDefaultBackground()
    {
        return new SolidColorBrush(Colors.Black)
        {
            Opacity = 0.3
        };
    }
}
