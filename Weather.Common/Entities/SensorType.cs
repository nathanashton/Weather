using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Weather.Common.Interfaces;
using Weather.Common.Units;
using static System.String;

namespace Weather.Common.Entities
{
    [ImplementPropertyChanged]
    public class SensorType : IDataErrorInfo, ISensorType
    {
        public int SensorTypeId { get; set; }
        public string Name { get; set; }
        public Unit SIUnit { get; set; }
        public IList<Unit> Units { get; set; } = new ObservableCollection<Unit>();
        public bool IsValid => Validate();

        public override string ToString()
        {
            return Name;
        }

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
                if (columnName == "SIUnit")
                {
                    if (SIUnit == null)
                    {
                        return "SI Unit is required";
                    }
                }
                return null;
            }
        }

        public string Error
        {
            get { throw new NotImplementedException(); }
        }

        private bool Validate()
        {
            var f = !IsNullOrEmpty(Name) && SIUnit != null;
            return f;
        }
    }
}