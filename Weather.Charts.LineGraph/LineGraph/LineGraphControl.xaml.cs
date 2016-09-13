using System.Windows;
using System.Windows.Input;
using PropertyChanged;
using Weather.Common.Interfaces;
using Weather.Interfaces;

namespace Weather.Charts.LineGraph
{
    /// <summary>
    ///     Interaction logic for LineGraphControl.xaml
    /// </summary>
    [ImplementPropertyChanged]
    public partial class LineGraphControl : IChartCodeBehind
    {
        public LineGraphControl(ISelectedStation selectedStation)
        {
            InitializeComponent();

            ViewModel = new LineGraphControlViewModel(selectedStation);

            DataContext = ViewModel;
            Loaded += MinMax_Loaded;
            Unloaded += MinMax_Unloaded;
        }

        public IChartViewModel ViewModel { get; set; }

        private void MinMax_Unloaded(object sender, RoutedEventArgs e)
        {
            ViewModel.SelectedStation.SelectedStationRecordsUpdated -= ViewModel.SelectedStation_RecordsUpdated;
            ViewModel.SelectedStation.GetRecordsCompleted -= ViewModel.SelectedStation_GetRecordsCompleted;
            ViewModel.SelectedStation.SelectedStationChanged -= ViewModel.SelectedStation_SelectedStationChanged;
        }


        private void MinMax_Loaded(object sender, RoutedEventArgs e)
        {
            ViewModel.SelectedStation.SelectedStationRecordsUpdated += ViewModel.SelectedStation_RecordsUpdated;
            ViewModel.SelectedStation.GetRecordsCompleted += ViewModel.SelectedStation_GetRecordsCompleted;
            ViewModel.SelectedStation.SelectedStationChanged += ViewModel.SelectedStation_SelectedStationChanged;
        }

        private void Label_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            //     ViewModel.SelectedSensor = null;
        }

        private void Label_MouseLeftButtonUp_1(object sender, MouseButtonEventArgs e)
        {
            //  ViewModel.SelectedSensor2 = null;
        }
    }
}