using System;
using System.Collections.ObjectModel;
using System.Linq;
using PropertyChanged;
using Weather.Common.Interfaces;
using Weather.Common.Units;
using Weather.Core.Interfaces;

namespace Weather.UserControls.Charts
{
    [ImplementPropertyChanged]
    public class AverageWindDirectionViewModel
    {
        private IWeatherRecordCore _weatherRecordCore;
        public ISelectedStation SelectedStation { get; set; }
        public ObservableCollection<ISensor> Sensors { get; set; }
        public string Header => "Average Wind Direction";

        public AverageWindDirectionViewModel(ISelectedStation selectedStation, IWeatherRecordCore weatherRecordCore)
        {
            _weatherRecordCore = weatherRecordCore;
            SelectedStation = selectedStation;
            Sensors = new ObservableCollection<ISensor>();
            SelectedStation.SelectedStationsChanged += SelectedStation_SelectedStationsChanged;
        }

        private void SelectedStation_SelectedStationsChanged(object sender, EventArgs e)
        {
            GetCompatibleUnits();
        }

        public void GetCompatibleUnits()
        {
            Sensors.Clear();
            if (SelectedStation.WeatherStation?.Sensors == null)
            {
                return;
            }
            var compatibleUnits = UnitTypes.UnitsList.First(x => x.Name == "Wind Direction");

            foreach (var s in SelectedStation.WeatherStation.Sensors)
            {
                if (s.Sensor.SensorType.SIUnit.UnitType == compatibleUnits)
                {
                    Sensors.Add(s.Sensor);
                }
            }
        }
    }
}