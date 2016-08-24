using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Weather.Common.Entities;
using Weather.ViewModels;

namespace Weather.Views
{
    /// <summary>
    ///     Interaction logic for StationWindow.xaml
    /// </summary>
    public partial class StationWindow
    {
        private readonly StationWindowViewModel _viewModel;

        public StationWindow(StationWindowViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            DataContext = viewModel;
            viewModel.StationWindow = this;
            Loaded += StationWindow_Loaded;
        }

        private void StationWindow_Loaded(object sender, RoutedEventArgs e)
        {
            //    _viewModel.RegisterDirtyHandlers();
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_viewModel.SelectedStation == null) return;
            var weatherStation = ((ListBox) e.Source).SelectedItem as WeatherStation;
            if (weatherStation == null) return;
            if (_viewModel.IsDirty)
            {
                var result =
                    MessageBox.Show(
                        "Save changes to " + _viewModel.SelectedStation.Manufacturer + " " +
                        _viewModel.SelectedStation.Model + "?", "Save Changes", MessageBoxButton.YesNo,
                        MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    _viewModel.SaveSpecificStation(_viewModel.SelectedStation);
                }
            }

            _viewModel.SelectedStation = weatherStation;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (_viewModel.IsDirty)
            {
                var result =
                    MessageBox.Show(
                        "Save changes to " + _viewModel.SelectedStation.Manufacturer + " " +
                        _viewModel.SelectedStation.Model + "?", "Save Changes", MessageBoxButton.YesNo,
                        MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    _viewModel.SaveSpecificStation(_viewModel.SelectedStation);
                }
            }
            Close();
        }

        private void DataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender != null)
            {
                var grid = sender as DataGrid;
                if (grid != null && grid.SelectedItems != null && grid.SelectedItems.Count == 1)
                {
                    var dgr = grid.ItemContainerGenerator.ContainerFromItem(grid.SelectedItem) as DataGridRow;
                    _viewModel.SelectedSensor = (Sensor) dgr.Item;
                    _viewModel.EditSensor(null);
                }
            }
        }
    }
}