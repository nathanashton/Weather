﻿using System.Collections.Generic;
using Weather.Common.Entities;

namespace Weather.Common.Interfaces
{
    public interface IWeatherStation
    {
        int WeatherStationId { get; set; }
        string Manufacturer { get; set; }
        string Model { get; set; }
        double? Latitude { get; set; }
        double? Longitude { get; set; }
        string Description { get; set; }
        bool IsValid { get; }

        IList<IStationSensor> Sensors { get; set; }


    }
}