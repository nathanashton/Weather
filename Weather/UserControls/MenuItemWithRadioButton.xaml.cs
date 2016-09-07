using System.Windows;
using System.Windows.Controls;

namespace Weather.UserControls
{
    /// <summary>
    ///     Interaction logic for MenuItemWithRadioButton.xaml
    /// </summary>
    public partial class MenuItemWithRadioButton
    {
        public MenuItemWithRadioButton()
        {
            InitializeComponent();
        }

        private void MenuItemWithRadioButtons_Click(object sender, RoutedEventArgs e)
        {
            var mi = sender as MenuItem;
            var rb = mi?.Icon as RadioButton;
            if (rb != null)
            {
                rb.IsChecked = true;
            }
        }
    }
}