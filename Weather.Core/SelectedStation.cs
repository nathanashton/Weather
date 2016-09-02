using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Weather.Common.Interfaces;
using Weather.Core.Interfaces;

namespace Weather.Core
{
    [ImplementPropertyChanged]
    public class SelectedStation : ISelectedStation
    {
        public IWeatherStation WeatherStation { get; set; }
        public event EventHandler StationsChanged;


        public void OnStationsChanged()
        {
            StationsChanged?.Invoke(this, null);
        }
    }
}
