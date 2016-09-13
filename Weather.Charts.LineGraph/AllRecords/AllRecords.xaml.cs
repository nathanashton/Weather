using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Weather.Common.Interfaces;
using Weather.Interfaces;

namespace Weather.Charts.AllRecords
{
    /// <summary>
    ///     Interaction logic for AllRecords.xaml
    /// </summary>
    public partial class AllRecords : UserControl, IChartCodeBehind
    {
        public AllRecords(ISelectedStation selectedStation)
        {
            InitializeComponent();

            ViewModel = new AllRecordsViewModel(selectedStation);

            DataContext = ViewModel;
            Loaded += AllRecords_Loaded;
            Unloaded += AllRecords_Unloaded;
        }

        public IChartViewModel ViewModel { get; set; }

        private void AllRecords_Unloaded(object sender, RoutedEventArgs e)
        {
            ViewModel.SelectedStation.SelectedStationChanged -= ViewModel.SelectedStation_SelectedStationChanged;
            ViewModel.SelectedStation.SelectedStationRecordsUpdated -= ViewModel.SelectedStation_RecordsUpdated;
            ViewModel.SelectedStation.GetRecordsCompleted -= ViewModel.SelectedStation_GetRecordsCompleted;
        }

        private void AllRecords_Loaded(object sender, RoutedEventArgs e)
        {
            ViewModel.SelectedStation.SelectedStationChanged += ViewModel.SelectedStation_SelectedStationChanged;
            ViewModel.SelectedStation.SelectedStationRecordsUpdated += ViewModel.SelectedStation_RecordsUpdated;
            ViewModel.SelectedStation.GetRecordsCompleted += ViewModel.SelectedStation_GetRecordsCompleted;
            ViewModel.DrawChart();
        }

        public void RenderGrid()
        {
            if (ViewModel.SelectedStation?.WeatherStation == null)
            {
                return;
            }
            dg.Columns.Clear();

            var columns =
                ViewModel.SelectedStation.WeatherStation.Sensors.Select((x, i) => new {x.Sensor.ShortName, Index = i})
                    .ToArray();

            dg.Columns.Add(new DataGridTextColumn {Header = "Time", Binding = new Binding("TimeStamp")});

            foreach (var column in columns)
            {
                var binding = new Binding($"SensorValues[{column.Index}].");
                string sort = $"SensorValues[{column.Index}].CorrectedValue";
                dg.Columns.Add(new DataGridTextColumn
                {
                    Header = column.ShortName,
                    Binding = binding,
                    SortMemberPath = sort
                });
            }

            dg.ItemsSource = ViewModel.SelectedStation.WeatherStation.Records;

            //Resize
            foreach (var column in dg.Columns)
            {
                column.MinWidth = column.ActualWidth;
                column.Width = new DataGridLength(1, DataGridLengthUnitType.Star);
            }
        }
    }
}