using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Weather.Common.Interfaces;

namespace Weather.Core.Interfaces
{
   public interface ISelectedStation
    {
        IWeatherStation WeatherStation { get; set; }

        event EventHandler StationsChanged;
        void OnStationsChanged();
    }
}
