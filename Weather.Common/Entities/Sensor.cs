using System;
using System.Collections.Generic;
using System.ComponentModel;
using PropertyChanged;
using Weather.Common.Interfaces;
using static System.String;

namespace Weather.Common.Entities
{
    [ImplementPropertyChanged]
    public class Sensor : ISensor, IDataErrorInfo
    {
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
                if (columnName == "SensorType")
                {
                    if (SensorType == null)
                    {
                        return "SensorType is required";
                    }
                }
                return null;
            }
        }

        public string Error
        {
            get { throw new NotImplementedException(); }
        }

        public long SensorId { get; set; }
        public string Manufacturer { get; set; }
        public string Model { get; set; }
        public string Description { get; set; }
        public ISensorType SensorType { get; set; }
        public long SensorTypeId { get; set; }
        public IList<ISensorValue> SensorValues { get; set; } = new List<ISensorValue>();

        public string FullName => ToString() + " (" + SensorType.Name + ")";
        public string ShortName => ToString();

        public bool IsValid => Validate();

        public override string ToString()
        {
            return Manufacturer + " " + Model;
        }

        private bool Validate()
        {
            var f = !IsNullOrEmpty(Manufacturer) && !IsNullOrEmpty(Model) && (SensorType != null);
            return f;
        }
    }
}