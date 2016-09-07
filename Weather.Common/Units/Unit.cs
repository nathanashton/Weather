using System;
using System.ComponentModel;

namespace Weather.Common.Units
{
    public class Unit : IDataErrorInfo
    {
        public int UnitId { get; set; }
        public UnitType UnitType { get; set; }
        public string DisplayName { get; set; }
        public string DisplayUnit { get; set; }

        public bool IsValid => Validate();

        public string this[string columnName]
        {
            get
            {
                if (columnName == "DisplayName")
                {
                    if (string.IsNullOrEmpty(DisplayName))
                    {
                        return "Name is required";
                    }
                }
                if (columnName == "DisplayUnit")
                {
                    if (string.IsNullOrEmpty(DisplayUnit))
                    {
                        return "Unit is required";
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
            return DisplayName + " (" + DisplayUnit + ")";
        }

        private bool Validate()
        {
            var f = !string.IsNullOrEmpty(DisplayName) && !string.IsNullOrEmpty(DisplayUnit) && (UnitType != null);
            return f;
        }
    }
}