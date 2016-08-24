using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media.Animation;
using System.Windows.Threading;
using Weather.Common.Entities;
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
            _viewModel.MainWindow = this;
            DataContext = _viewModel;
            Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            CreateDataGrid();
            DispatcherTimer timer = new DispatcherTimer(new TimeSpan(0, 0, 1), DispatcherPriority.Normal, delegate
            {
                _viewModel.Clock = DateTime.Now.ToString();
            }, this.Dispatcher);
        }


        private void CreateDataGrid()
        {
            cmb.SelectedItem = _viewModel.SelectedStation;
            dg.Columns.Clear();
            if (_viewModel.SelectedStation == null) return;
            var columns = _viewModel.SelectedStation
                .Sensors.Select((x, i) => new {x.Model, Index = i}).ToArray();
            dg.Columns.Add(new DataGridTextColumn {Header = "Time", Binding = new Binding("TimeStamp")});

            foreach (var column in columns)
            {
                var binding = new Binding($"SensorValues[{column.Index}].");
                string sort = $"SensorValues[{column.Index}].RawValue";
                dg.Columns.Add(new DataGridTextColumn {Header = column.Model, Binding = binding, SortMemberPath = sort});
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
            var weatherStation = ((ComboBox) e.Source).SelectedItem as WeatherStation;
            if (weatherStation == null) return;
            _viewModel.SelectedStation = weatherStation;
            CreateDataGrid();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

        }
    }
}