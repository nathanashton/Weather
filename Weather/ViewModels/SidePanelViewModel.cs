using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;
using PropertyChanged;
using Weather.Common;
using Weather.Common.Interfaces;
using Weather.Core.Interfaces;

namespace Weather.ViewModels
{
    [ImplementPropertyChanged]
    public class SidePanelViewModel : NotifyBase
    {
        private readonly IStationCore _stationCore;
        private readonly IWeatherRecordCore _weatherRecordCore;
        private IWeatherStation _selected;
        private ISelectedStation _selectedStation;

        public ObservableCollection<IWeatherStation> Stations { get; set; }

        public ISelectedStation SelectedStation
        {
            get { return _selectedStation; }
            set
            {
                _selectedStation = value;

                OnPropertyChanged(() => SelectedStation);
            }
        }

        public ICommand FloatingActionDemoCommand { get; }

        public bool Loading { get; set; }

        public IWeatherStation Selected // TODO
        {
            get { return _selected; }
            set
            {
                _selected = value;
                SelectedStation.WeatherStation = value;
                Dispatcher.CurrentDispatcher.InvokeAsync(async () => await GetRecords());

                OnPropertyChanged(() => Selected);
            }
        }

        public SidePanelViewModel(IStationCore stationCore, ISelectedStation selectedStation,
            IWeatherRecordCore weatherRecordCore)
        {
            _weatherRecordCore = weatherRecordCore;
            SelectedStation = selectedStation;
            _stationCore = stationCore;
            
            SelectedStation.RecordsUpdatedForSelectedStation += RecordsUpdatedForSelectedStation;
            SelectedStation.SelectedStationChanged += SelectedStation_SelectedStationChanged;
            SelectedStation.ChangesMadeToSelectedStation += ChangesMadeToSelectedStation;
            SelectedStation.GetRecordsStarted += SelectedStation_GetRecordsStarted;
            SelectedStation.GetRecordsCompleted += SelectedStation_GetRecordsCompleted;
            
        }

        private void SelectedStation_GetRecordsCompleted(object sender, EventArgs e)
        {
            Loading = false;
        }

        private void SelectedStation_GetRecordsStarted(object sender, EventArgs e)
        {
            Loading = true;
        }

      
        private async void RecordsUpdatedForSelectedStation(object sender, EventArgs e)
        {
            await GetRecords();
        }

        private void ChangesMadeToSelectedStation(object sender, EventArgs e)
        {
            long id = 0;
            if ((SelectedStation != null) && (SelectedStation.WeatherStation != null))
            {
                id = SelectedStation.WeatherStation.WeatherStationId;
            }
            GetAllStations();
            var found = Stations.FirstOrDefault(x => x.WeatherStationId == id);
            if (found != null)
            {
                Selected = found;
            }
        }

        private void SelectedStation_SelectedStationChanged(object sender, EventArgs e)
        {
            Selected = SelectedStation.WeatherStation;
        }


        public async Task GetRecords()
        {
            if ((SelectedStation.StartDate == null) || (SelectedStation.EndDate == null))
            {
                return;
            }

            if (_selectedStation?.WeatherStation == null)
            {
                return;
            }


            var s = SelectedStation.StartDate;
            var e = SelectedStation.EndDate;
            SelectedStation.OnGetRecordsStarted();

            await
                Task.Run(
                    async () =>
                    {
                        SelectedStation.WeatherStation.Records =
                            await
                                _weatherRecordCore.GetAllRecordsForStationBetweenDates(
                                        SelectedStation.WeatherStation.WeatherStationId, (DateTime) s, (DateTime) e)
                                    .ConfigureAwait(true);
                    });
            SelectedStation.OnGetRecordsCompleted();
        }

        public void GetAllStations()
        {
            Stations = new ObservableCollection<IWeatherStation>(_stationCore.GetAllStations());
            SelectedStation.OnSelectedStationChanged();
        }
    }
}