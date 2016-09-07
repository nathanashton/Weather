using System.Windows;
using System.Windows.Input;
using Weather.ViewModels;

namespace Weather.Views
{
    /// <summary>
    ///     Interaction logic for UnhandledExceptionWindow.xaml
    /// </summary>
    public partial class UnhandledExceptionWindow
    {
        public UnhandledExceptionWindowViewModel ViewModel;

        public UnhandledExceptionWindow(UnhandledExceptionWindowViewModel viewModel)
        {
            InitializeComponent();
            ViewModel = viewModel;
            ViewModel.Window = this;
            DataContext = ViewModel;
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

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}