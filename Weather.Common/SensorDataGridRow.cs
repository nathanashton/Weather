using PropertyChanged;
using System;
using System.Collections.Generic;
using Weather.Common.Interfaces;

namespace Weather.Common
{
    [ImplementPropertyChanged]
    public class SensorDataGridRow
    {
        public DateTime TimeStamp { get; set; }
        public List<ISensorValue> Sensors { get; set; }
    }
}