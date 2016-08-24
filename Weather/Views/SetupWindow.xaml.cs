using System.Windows;
using PropertyChanged;
using Weather.ViewModels;

namespace Weather.Views
{
    /// <summary>
    ///     Interaction logic for SetupWindow.xaml
    /// </summary>
    [ImplementPropertyChanged]
    public partial class SetupWindow : Window
    {
        private readonly SetupWindowViewModel _viewModel;

        public SetupWindow(SetupWindowViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            DataContext = _viewModel;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}