using System.Windows;
using Weather.Common.Interfaces;

namespace Weather.Charts.WindPolar
{
    /// <summary>
    ///     Interaction logic for WindPolarControl.xaml
    /// </summary>
    public partial class WindPolarControl : IChartCodeBehind
    {
        public WindPolarControl(ISelectedStation selectedStation)
        {
            InitializeComponent();
            ViewModel = new WindPolarControlViewModel(selectedStation);
            DataContext = ViewModel;
            Loaded += WindPolarControl_Loaded;
            Unloaded += WindPolarControl_Unloaded;
        }

        public IChartViewModel ViewModel { get; set; }

        private void WindPolarControl_Unloaded(object sender, RoutedEventArgs e)
        {
            ViewModel.SelectedStation.RecordsUpdatedForSelectedStation -= ViewModel.RecordsUpdatedForSelectedStation;
            ViewModel.SelectedStation.GetRecordsCompleted -= ViewModel.SelectedStation_GetRecordsCompleted;
            ViewModel.SelectedStation.SelectedStationChanged -= ViewModel.SelectedStation_SelectedStationChanged;
            ViewModel.SelectedStation.ChangesMadeToSelectedStation -= ViewModel.ChangesMadeToSelectedStation;

            ViewModel.SavePosition();
        }

        private void WindPolarControl_Loaded(object sender, RoutedEventArgs e)
        {
            ViewModel.SelectedStation.RecordsUpdatedForSelectedStation += ViewModel.RecordsUpdatedForSelectedStation;
            ViewModel.SelectedStation.GetRecordsCompleted += ViewModel.SelectedStation_GetRecordsCompleted;
            ViewModel.SelectedStation.SelectedStationChanged += ViewModel.SelectedStation_SelectedStationChanged;
            ViewModel.SelectedStation.ChangesMadeToSelectedStation += ViewModel.ChangesMadeToSelectedStation;

            ViewModel.LoadPosition();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.OptionsOpened = false;
        }
    }
}