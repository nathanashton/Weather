using System.Windows;
using System.Windows.Controls;
using Weather.Common.Entities;
using Weather.Common.Interfaces;
using Weather.Common.Units;
using Weather.ViewModels;

namespace Weather.Views
{
    /// <summary>
    /// Interaction logic for SensorTypesWindow.xaml
    /// </summary>
    public partial class UnitsWindow : Window
    {
        private readonly UnitsWindowViewModel _viewModel;

        public UnitsWindow(UnitsWindowViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            DataContext = _viewModel;
            _viewModel.UnitsWindow = this;
            Loaded += SensorTypesWindow_Loaded;
        }

        private void SensorTypesWindow_Loaded(object sender, RoutedEventArgs e)
        {
            _viewModel.RegisterDirtyHandlers();
        }

        private void ListBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (_viewModel.SelectedUnit == null) return;
            var selection = ((ListBox)e.Source).SelectedItem as Unit;
            if (selection == null) return;
            if (_viewModel.IsDirty)
            {
                var result =
                    MessageBox.Show(
                        "Save changes to " + _viewModel.SelectedUnit.DisplayName + "?", "Save Changes", MessageBoxButton.YesNo,
                        MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    _viewModel.Save(null);
                }
            }

            _viewModel.SelectedUnit = selection;
        }

        private void UnitsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}