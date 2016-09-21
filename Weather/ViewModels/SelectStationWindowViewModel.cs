using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using PropertyChanged;
using Weather.Common.Interfaces;
using Weather.Core.Interfaces;
using Weather.Helpers;

namespace Weather.ViewModels
{
    [ImplementPropertyChanged]
    public class SelectStationWindowViewModel
    {
        private readonly IStationCore _stationCore;
        public Window Window { get; set; }
        public IWeatherStation SelectedStation { get; set; }
        public ObservableCollection<IWeatherStation> Stations { get; set; }
        public ISelectedStation SStation { get; set; }

        public ICommand SelectCommand
        {
            get { return new RelayCommand(Selected, x => SelectedStation != null); }
        }

        public SelectStationWindowViewModel(IStationCore stationCore, ISelectedStation selectedStation)
        {
            _stationCore = stationCore;
            SStation = selectedStation;
        }

        public void GetAllStations()
        {
            Stations = new ObservableCollection<IWeatherStation>(_stationCore.GetAllStations());
        }

        private void Selected(object obj)
        {
            Window.Close();
        }
    }
}