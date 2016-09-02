using System.Windows.Controls;

namespace Weather.UserControls
{
    /// <summary>
    /// Interaction logic for MenuItemWithRadioButton.xaml
    /// </summary>
    public partial class MenuItemWithRadioButton
    {
        public MenuItemWithRadioButton()
        {
            InitializeComponent();
        }

        private void MenuItemWithRadioButtons_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            MenuItem mi = sender as MenuItem;
            if (mi != null)
            {
                RadioButton rb = mi.Icon as RadioButton;
                if (rb != null)
                {
                    rb.IsChecked = true;
                }
            }
        }
    }
}
