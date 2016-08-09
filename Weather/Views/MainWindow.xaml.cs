using Microsoft.Practices.Unity;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Weather.Common.Entities;
using Weather.DependencyResolver;
using Weather.ViewModels;

namespace Weather.Views
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private readonly MainWindowViewModel _viewModel;

        public MainWindow(MainWindowViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            DataContext = _viewModel;
            Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            CreateDataGrid();
        }


        private void CreateDataGrid()
        {
            cmb.SelectedItem = _viewModel.SelectedStation;
            dg.Columns.Clear();
            if (_viewModel.SelectedStation == null) return;
            var columns = _viewModel.SelectedStation
              .Sensors.Select((x, i) => new { x.Name, Index = i }).ToArray();
            dg.Columns.Add(new DataGridTextColumn() { Header = "Time", Binding = new Binding("TimeStamp") });

            foreach (var column in columns)
            {
                var binding = new Binding($"SensorValues[{column.Index}].");
                string sort = $"SensorValues[{column.Index}].RawValue";
                dg.Columns.Add(new DataGridTextColumn() { Header = column.Name, Binding = binding, SortMemberPath = sort });
            }

            dg.ItemsSource = _viewModel.SelectedStation.WeatherRecords;

            //Resize
            foreach (var column in dg.Columns)
            {
                column.MinWidth = column.ActualWidth;
                column.Width = new DataGridLength(1, DataGridLengthUnitType.Star);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            CreateDataGrid();

        }

        private void lb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var weatherStation = ((ComboBox)e.Source).SelectedItem as WeatherStation;
            if (weatherStation == null) return;
            _viewModel.SelectedStation = weatherStation;
            CreateDataGrid();
        }


    }
}