using System;
using System.ComponentModel;
using PropertyChanged;
using Weather.Common.Interfaces;
using Weather.Units.Interfaces;
using static System.String;

namespace Weather.Common.Entities
{
    [ImplementPropertyChanged]
    public class SensorType : IDataErrorInfo, ISensorType
    {
        public string this[string columnName]
        {
            get
            {
                if (columnName == "Name")
                {
                    if (IsNullOrEmpty(Name))
                    {
                        return "Name is required";
                    }
                }
             
                return null;
            }
        }

        public string Error
        {
            get { throw new NotImplementedException(); }
        }

        public int SensorTypeId { get; set; }
        public string Name { get; set; }
        public IUnitType UnitType { get; set; }

        public bool IsValid => Validate();

        public override string ToString()
        {
            return Name;
        }

        private bool Validate()
        {
           ;
            return !IsNullOrEmpty(Name) && UnitType != null;
        }
    }
}