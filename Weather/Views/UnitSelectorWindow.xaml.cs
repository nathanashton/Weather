using System.Windows;
using System.Windows.Input;
using Weather.ViewModels;

namespace Weather.Views
{
    /// <summary>
    ///     Interaction logic for UnitSelectorWindow.xaml
    /// </summary>
    public partial class UnitSelectorWindow
    {
        public UnitSelectorWindowViewModel ViewModel { get; set; }

        public UnitSelectorWindow(UnitSelectorWindowViewModel viewModel)
        {
            InitializeComponent();
            ViewModel = viewModel;
            DataContext = ViewModel;
            ViewModel.Window = this;
            Loaded += UnitSelectorWindow_Loaded;
        }

        private void UnitSelectorWindow_Loaded(object sender, RoutedEventArgs e)
        {
            ViewModel.GetAllUnits();
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