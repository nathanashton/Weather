using System.Linq;
using System.Windows.Controls;
using System.Windows.Data;
using Microsoft.Practices.Unity;
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
        private MainWindowViewModel vm;

        public MainWindow(MainWindowViewModel windowViewModel)
        {
            InitializeComponent();
            vm = windowViewModel;
            DataContext = vm;

            vm.Go();
            CreateDataGrid();
            Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {

        }

        private void Button_Click_1(object sender, System.Windows.RoutedEventArgs e)
        {
            //foreach (var record in vm.Station.WeatherRecords)
            //{
            //    foreach (var sensor in record.SensorValues)
            //    {
            //        var temperature = sensor as ITemperature;
            //        temperature?.DisplayDegreesFahrenheit();

            //        var pressure = sensor as IPressure;
            //        pressure?.DisplayInHg();
            //    }
            //}
            //dg.Items.Refresh();
        }

        private void CreateDataGrid()
        {
            //var columns = vm.Station
            //  .Sensors.Select((x, i) => new { x.Name, Index = i }).ToArray();
            //dg.Columns.Add(new DataGridTextColumn() { Header = "Time", Binding = new Binding("TimeStamp") });

            //foreach (var column in columns)
            //{
            //    var binding = new Binding($"SensorValues[{column.Index}].");
            //    string sort = $"SensorValues[{column.Index}].Value";
            //    dg.Columns.Add(new DataGridTextColumn() { Header = column.Name, Binding = binding, SortMemberPath = sort });
            //}

            //dg.ItemsSource = vm.Station.WeatherRecords;

            ////Resize
            //foreach (var column in dg.Columns)
            //{
            //    column.MinWidth = column.ActualWidth;
            //    column.Width = new DataGridLength(1, DataGridLengthUnitType.Star);
            //}
        }

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var container = Resolver.Bootstrap();
            var window = container.Resolve<StationWindow>();
            window.ShowDialog();
        }
    }
}