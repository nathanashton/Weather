using System.Windows;
using Weather.ViewModels;

namespace Weather.Views
{
    /// <summary>
    ///     Interaction logic for UnitSelectorWindow.xaml
    /// </summary>
    public partial class UnitSelectorWindow : Window
    {
        public UnitSelectorWindow(UnitSelectorWindowViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            DataContext = _viewModel;
            _viewModel.Window = this;
            Loaded += UnitSelectorWindow_Loaded;
        }

        public UnitSelectorWindowViewModel _viewModel { get; set; }

        private void UnitSelectorWindow_Loaded(object sender, RoutedEventArgs e)
        {
            _viewModel.GetAllUnits();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}