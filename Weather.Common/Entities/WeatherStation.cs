using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using PropertyChanged;
using Weather.Common.Interfaces;

namespace Weather.Common.Entities
{
    [ImplementPropertyChanged]
    public class WeatherStation : IWeatherStation, IDataErrorInfo
    {
        public WeatherStation()
        {
            Sensors = new List<IStationSensor>();
        }


        public string this[string columnName]
        {
            get
            {
                if (columnName == "Manufacturer")
                {
                    if (string.IsNullOrEmpty(Manufacturer))
                    {
                        return "Manufacturer is required";
                    }
                }
                if (columnName == "Model")
                {
                    if (string.IsNullOrEmpty(Model))
                    {
                        return "Model is required";
                    }
                }
                return null;
            }
        }

        public string Error
        {
            get { throw new NotImplementedException(); }
        }

        public string Description { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public string Manufacturer { get; set; }
        public string Model { get; set; }
        public IList<IStationSensor> Sensors { get; set; }
        public ObservableCollection<IWeatherRecord> Records { get; set; } = new ObservableCollection<IWeatherRecord>();
        public int WeatherStationId { get; set; }
        public bool IsValid => Validate();

        public override string ToString()
        {
            return Manufacturer + " " + Model;
        }

        private bool Validate()
        {
            var f = !string.IsNullOrEmpty(Manufacturer) && !string.IsNullOrEmpty(Model);
            return f;
        }
    }
}