using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Weather.Common.Interfaces;
using Weather.Core.Interfaces;

namespace Weather.ViewModels
{
    [ImplementPropertyChanged]
    public class StationPanelViewModel
    {
        public IWeatherStation Station { get; set;}
        public string Sensors
        {
            get { return GetSensors(); }
        }

        public StationPanelViewModel(IStationCore stationCore)
        {
            Station = stationCore.GetAllStations().First();
        }

        private string GetSensors()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var stationsensor in Station.Sensors)
            {
                sb.Append(stationsensor.ToString());
                sb.Append(Environment.NewLine);
            }
            return sb.ToString();
        }
    }
}
