using System.Linq;
using System.Windows.Controls;
using System.Windows.Data;
using Microsoft.Practices.Unity;
using Weather.Core;
using Weather.DependencyResolver;

namespace Weather.UserControls.Charts
{
    /// <summary>
    ///     Interaction logic for AllRecords.xaml
    /// </summary>
    public partial class AllRecords : UserControl
    {
        private readonly AllRecordsViewModel _viewModel;

        public AllRecords()
        {
            InitializeComponent();
            var container = new Resolver().Bootstrap();
            _viewModel = container.Resolve<AllRecordsViewModel>();
            _viewModel.Window = this;
            DataContext = _viewModel;
            Loaded += AllRecords_Loaded; Unloaded += AllRecords_Unloaded;
        }

        private void AllRecords_Unloaded(object sender, System.Windows.RoutedEventArgs e)
        {
            _viewModel.SelectedStation.SelectedStationChanged -= _viewModel.SelectedStation_SelectedStationsChanged;
            _viewModel.SelectedStation.SelectedStationRecordsUpdated -= _viewModel.SelectedStation_SelectedStationRecordsUpdated;
            _viewModel.SelectedStation.GetRecordsCompleted -= _viewModel.SelectedStation_GetRecordsCompleted;

        }

        private void AllRecords_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            _viewModel.SelectedStation.SelectedStationChanged += _viewModel.SelectedStation_SelectedStationsChanged;
            _viewModel.SelectedStation.SelectedStationRecordsUpdated += _viewModel.SelectedStation_SelectedStationRecordsUpdated;
            _viewModel.SelectedStation.GetRecordsCompleted += _viewModel.SelectedStation_GetRecordsCompleted;
            _viewModel.Draw();
        }



        public void RenderGrid()
        {
            if (_viewModel.SelectedStation?.WeatherStation == null)
            {
                return;
            }
            dg.Columns.Clear();

            var columns =
                _viewModel.SelectedStation.WeatherStation.Sensors.Select((x, i) => new {x.Sensor.ShortName, Index = i})
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

            dg.ItemsSource = _viewModel.SelectedStation.WeatherStation.Records;

            //Resize
            foreach (var column in dg.Columns)
            {
                column.MinWidth = column.ActualWidth;
                column.Width = new DataGridLength(1, DataGridLengthUnitType.Star);
            }
        }
    }
}