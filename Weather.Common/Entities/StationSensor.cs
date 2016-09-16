using System;
using System.ComponentModel;
using PropertyChanged;
using Weather.Common.Interfaces;

namespace Weather.Common.Entities
{
    [ImplementPropertyChanged]
    public class StationSensor : IStationSensor
    {
        public long StationSensorId { get; set; }
        public double Correction { get; set; }
        public string Notes { get; set; }
        public ISensor Sensor { get; set; }

        public override string ToString()
        {
            return Sensor.ToString();
        }

    }
}