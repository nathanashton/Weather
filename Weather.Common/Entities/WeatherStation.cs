using PropertyChanged;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using Weather.Common.Interfaces;
using static System.String;

namespace Weather.Common.Entities
{
    [ImplementPropertyChanged]
    public class WeatherStation : IWeatherStation, IDataErrorInfo
    {
        public string Description { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public string Manufacturer { get; set; }
        public string Model { get; set; }
        public IList<IStationSensor> Sensors { get; set; }
        public int WeatherStationId { get; set; }
        public bool IsValid => Validate();

 

        public string this[string columnName]
        {
            get
            {
                if (columnName == "Manufacturer")
                {
                    if (IsNullOrEmpty(Manufacturer))
                    {
                        return "Manufacturer is required";
                    }
                }
                if (columnName == "Model")
                {
                    if (IsNullOrEmpty(Model))
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

        public override string ToString()
        {
            return Manufacturer + " " + Model;
        }

        private bool Validate()
        {
            var f = !IsNullOrEmpty(Manufacturer) && !IsNullOrEmpty(Model);
            return f;
        }

        public WeatherStation()
        {
            Sensors = new List<IStationSensor>();
        }
    }
}