using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Microsoft.Practices.Unity;
using Weather.Common.Entities;
using Weather.Common.Interfaces;
using Weather.DependencyResolver;
using Weather.ViewModels;

namespace Weather.Views
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private readonly MainWindowViewModel vm;

        public MainWindow(MainWindowViewModel viewModel)
        {
            InitializeComponent();
            vm = viewModel;
            DataContext = vm;

            vm.Go();
            CreateDataGrid();
            Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var p = "t";


            if (vm.SelectedStation == null) return;
            //foreach (var sensor in vm.SelectedStation.Sensors)
            //{
            //    sensor.Correction = 10;
            //}
            foreach (var t in vm.SelectedStation.WeatherRecords)
            {
                foreach (var pp in t.SensorValues)
                {
                    if (pp is ITemperature)
                    {
                        ((ITemperature)pp).DisplayDegreesCelsius();
                    }
                }
            }

            dg.Items.Refresh();
        }

        private void CreateDataGrid()
        {
            dg.Columns.Clear();
            if (vm.SelectedStation == null) return;
            var columns = vm.SelectedStation
              .Sensors.Select((x, i) => new { x.Name, Index = i }).ToArray();
            dg.Columns.Add(new DataGridTextColumn() { Header = "Time", Binding = new Binding("TimeStamp") });

            foreach (var column in columns)
            {
                var binding = new Binding($"SensorValues[{column.Index}].");
                string sort = $"SensorValues[{column.Index}].Value";
                dg.Columns.Add(new DataGridTextColumn() { Header = column.Name, Binding = binding, SortMemberPath = sort });
            }

       
           dg.ItemsSource = vm.SelectedStation.WeatherRecords;
            

            //Resize
            foreach (var column in dg.Columns)
            {
                column.MinWidth = column.ActualWidth;
                column.Width = new DataGridLength(1, DataGridLengthUnitType.Star);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var container = Resolver.Bootstrap();
            var window = container.Resolve<StationWindow>();
            window.ShowDialog();
        }

        private void lb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var weatherStation = ((ListBox)e.Source).SelectedItem as WeatherStation;
            if (weatherStation == null) return;
            vm.SelectedStation = weatherStation;
            CreateDataGrid();

        }
    }
}