using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        public Window Window { get; set; }
        public IWeatherStation SelectedStation { get; set; }
        public ObservableCollection<IWeatherStation> Stations { get; set; }
        private readonly IStationCore _stationCore;
        public ISelectedStation SStation { get; set; }

        public SelectStationWindowViewModel(IStationCore stationCore, ISelectedStation selectedStation)
        {
            _stationCore = stationCore;
            SStation = selectedStation;
        }

        public void GetAllStations()
        {
            Stations = new ObservableCollection<IWeatherStation>(_stationCore.GetAllStations());
        }
        public ICommand SelectCommand
        {
            get { return new RelayCommand(Selected, x => SelectedStation != null); }
        }

        private void Selected(object obj)
        {
            Window.Close();
        }

    }
}
