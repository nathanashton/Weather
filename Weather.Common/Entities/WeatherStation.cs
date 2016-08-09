using PropertyChanged;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Weather.Common.Interfaces;

namespace Weather.Common.Entities
{
    [ImplementPropertyChanged]
    public class WeatherStation : IWeatherStation
    {
        public long Id { get; set; }
        public string Manufacturer { get; set; }
        public string Model { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Name => Manufacturer + " " + Model;
        public ICollection<WeatherRecord> WeatherRecords { get; set; } = new ObservableCollection<WeatherRecord>();
        public virtual ICollection<Sensor> Sensors { get; set; } = new ObservableCollection<Sensor>();

        public void AddSensor(Sensor sensor)
        {
            Sensors.Add(sensor);
        }

        public void AddRecord(WeatherRecord record)
        {
            WeatherRecords.Add(record);
        }

        public override string ToString()
        {
            return Manufacturer + " " + Model;
        }
    }
}