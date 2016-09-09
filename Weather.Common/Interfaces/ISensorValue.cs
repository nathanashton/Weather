﻿using Weather.Units;

namespace Weather.Common.Interfaces
{
    public interface ISensorValue
    {
        int SensorValueId { get; set; }
        double? RawValue { get; set; }
        double? CorrectedValue { get; }
        Unit DisplayUnit { get; set; }
        double? DisplayValue { get; set; }
        ISensor Sensor { get; set; }
        IWeatherStation Station { get; set; }
        int StationId { get; set; }
        int SensorId { get; set; }
    }
}