using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Weather.Common;
using Weather.Common.Interfaces;
using Weather.Core.Interfaces;

namespace Weather.Core
{
    public class SelectedStation : NotifyBase, ISelectedStation
    {
        private IWeatherStation _weatherStation;

        public IWeatherStation WeatherStation
        {
            get { return _weatherStation; }
            set
            {
                _weatherStation = value;
                OnPropertyChanged(() => WeatherStation);
                OnSelectedStationChanged();
            }
        }
        public event EventHandler StationsChanged;

        public event EventHandler SelectedStationsChanged;


        public void OnStationsChanged()
        {
            StationsChanged?.Invoke(this, null);
        }

        public void OnSelectedStationChanged()
        {
            SelectedStationsChanged?.Invoke(this, null);
        }
    }
}
