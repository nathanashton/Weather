using System.Windows;
using System.Windows.Input;
using Weather.ViewModels;

namespace Weather.Views
{
    /// <summary>
    ///     Interaction logic for OptionsWindow.xaml
    /// </summary>
    public partial class OptionsWindow
    {
        public OptionsWindow(OptionsWindowViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void DockPanel_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            OnMouseLeftButtonDown(e);
            DragMove();
        }
    }
}