using System;
using System.ComponentModel;
using static System.String;

namespace Weather.Common.Units
{
    public class Unit : IDataErrorInfo
    {
        public int UnitId { get; set; }
        public UnitType UnitType { get; set; }
        public string DisplayName { get; set; }
        public string DisplayUnit { get; set; }

        public bool IsValid => Validate();

        public override string ToString()
        {
            return DisplayName + " (" + DisplayUnit + ")";
        }

        public string this[string columnName]
        {
            get
            {
                if (columnName == "DisplayName")
                {
                    if (IsNullOrEmpty(DisplayName))
                    {
                        return "Name is required";
                    }
                }
                if (columnName == "DisplayUnit")
                {
                    if (IsNullOrEmpty(DisplayUnit))
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

        private bool Validate()
        {
            var f= !IsNullOrEmpty(DisplayName) && !IsNullOrEmpty(DisplayUnit) && UnitType != null;
            return f;
        }
    }
}